using System.Security.Claims;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using SquirrelsNest.Pecan.Client.Constants;
using SquirrelsNest.Pecan.Shared.Constants;

namespace SquirrelsNest.Pecan.Client.Auth.Support {
    public class AuthStateProvider : AuthenticationStateProvider {
        private readonly ILocalStorageService   mLocalStorage;
        private readonly AuthenticationState    mAnonymous;

        public AuthStateProvider( ILocalStorageService localStorage ) {
            mLocalStorage = localStorage;

            mAnonymous = new AuthenticationState( new ClaimsPrincipal( new ClaimsIdentity()));
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync() {
            var token = await mLocalStorage.GetItemAsStringAsync( LocalStorageNames.AuthToken );
            // var refreshToken = await mLocalStorage.GetItemAsStringAsync( LocalStorageNames.RefreshToken );

            // mAuthFacade.SetInitialToken( token, refreshToken );

            if( string.IsNullOrWhiteSpace( token )) {
                return mAnonymous;
            }

            return new AuthenticationState( 
                new ClaimsPrincipal( new ClaimsIdentity( JwtParser.ParseClaimsFromJwt( token ), JWTConstants.JwtAuthType )));
        }

        public void NotifyUserAuthentication( string authToken ) {
            var authenticatedUser = new ClaimsPrincipal(
                new ClaimsIdentity( JwtParser.ParseRolesFromJwt( authToken ), JWTConstants.JwtAuthType ));
            var authState = Task.FromResult( new AuthenticationState( authenticatedUser ));

            NotifyAuthenticationStateChanged( authState );
        }

        public void NotifyUserLogout() {
            var authState = Task.FromResult( mAnonymous );

            NotifyAuthenticationStateChanged( authState );
        }
    }
}
