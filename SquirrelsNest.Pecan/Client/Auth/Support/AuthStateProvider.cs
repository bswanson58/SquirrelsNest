using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace SquirrelsNest.Pecan.Client.Auth.Support {
    public class AuthStateProvider : AuthenticationStateProvider {
        private readonly ILocalStorageService   mLocalStorage;
        private readonly AuthenticationState    mAnonymous;

        public AuthStateProvider( ILocalStorageService localStorage ) {
            mLocalStorage = localStorage;

            mAnonymous = new AuthenticationState( new ClaimsPrincipal( new ClaimsIdentity()));
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync() {
            var token = await mLocalStorage.GetItemAsync<string>( "authToken" );

            if( string.IsNullOrWhiteSpace( token )) {
                return mAnonymous;
            }

            return new AuthenticationState( 
                new ClaimsPrincipal( new ClaimsIdentity( JwtParser.ParseClaimsFromJwt( token ), "jwtAuthType" )));
        }

        public void NotifyUserAuthentication( string authToken ) {
            var claims = JwtParser.ParseClaimsFromJwt( authToken );
            var email = claims.FirstOrDefault( c => c.Type.Equals( ClaimTypes.Email ))?.Value ?? String.Empty;
            var authenticatedUser = new ClaimsPrincipal(
                new ClaimsIdentity(new[] { new Claim( ClaimTypes.Name, email ) }, "jwtAuthType" ));
            var authState = Task.FromResult( new AuthenticationState( authenticatedUser ));

            NotifyAuthenticationStateChanged( authState );
        }

        public void NotifyUserLogout() {
            var authState = Task.FromResult( mAnonymous );

            NotifyAuthenticationStateChanged( authState );
        }
    }
}
