using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using SquirrelsNest.Pecan.Client.Constants;
using SquirrelsNest.Pecan.Shared.Constants;
using SquirrelsNest.Pecan.Shared.Platform;

namespace SquirrelsNest.Pecan.Client.Auth.Support {
    public class AuthStateProvider : AuthenticationStateProvider {
        private readonly ILocalStorageService   mLocalStorage;
        private readonly AuthenticationState    mAnonymous;

        public AuthStateProvider( ILocalStorageService localStorage ) {
            mLocalStorage = localStorage;

            mAnonymous = new AuthenticationState( new ClaimsPrincipal( new ClaimsIdentity()));
        }

        private AuthenticationState CreateAuthenticationState( string fromToken ) {
            return new AuthenticationState( 
                new ClaimsPrincipal( 
                    new ClaimsIdentity( JwtParser.GetClaims( fromToken ), JWTConstants.JwtAuthType )));
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync() {
            var token = await mLocalStorage.GetItemAsStringAsync( LocalStorageNames.AuthToken );
            var expiration = JwtParser.GetClaimValue( token, "exp" );

            if(!String.IsNullOrWhiteSpace( expiration )) {
                var expTime = DateTimeOffset.FromUnixTimeSeconds( Convert.ToInt64( expiration ));
                        
                if(( expTime - DateTimeProvider.Instance.CurrentUtcTime ) > TimeSpan.Zero ) {
                    return CreateAuthenticationState( token );
                }
            }

            return mAnonymous;
        }

        public async Task SetUserAuthentication( string authToken, string refreshToken ) {
            await mLocalStorage.SetItemAsStringAsync( LocalStorageNames.AuthToken, authToken );
            await mLocalStorage.SetItemAsStringAsync( LocalStorageNames.RefreshToken, refreshToken );

            NotifyAuthenticationStateChanged( GetAuthenticationStateAsync());
        }

        public void NotifyUserLogout() {
            var authState = Task.FromResult( mAnonymous );

            NotifyAuthenticationStateChanged( authState );
        }
    }
}
