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
            var refreshToken = await mLocalStorage.GetItemAsync<string>( "refreshToken" );

            // mAuthFacade.SetInitialToken( token, refreshToken );

            if( string.IsNullOrWhiteSpace( token )) {
                return mAnonymous;
            }

            return new AuthenticationState( 
                new ClaimsPrincipal( new ClaimsIdentity( JwtParser.ParseClaimsFromJwt( token ), "jwtAuthType" )));
        }

        public void NotifyUserAuthentication( string authToken ) {
            var authenticatedUser = new ClaimsPrincipal(
                new ClaimsIdentity( JwtParser.ParseRolesFromJwt( authToken ), "jwtAuthType" ));
            var authState = Task.FromResult( new AuthenticationState( authenticatedUser ));

            NotifyAuthenticationStateChanged( authState );
        }

        public void NotifyUserLogout() {
            var authState = Task.FromResult( mAnonymous );

            NotifyAuthenticationStateChanged( authState );
        }
    }
}
