using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using SquirrelsNest.Pecan.Client.Constants;

namespace SquirrelsNest.Pecan.Client.Auth.Support {
    public class JwtTokenHandler : DelegatingHandler {
        private readonly ITokenRefresher        mTokenRefresher;
        private readonly ILocalStorageService   mLocalStorage;

        public JwtTokenHandler( ITokenRefresher tokenRefresher, ILocalStorageService storageService ) {
            mTokenRefresher = tokenRefresher;
            mLocalStorage = storageService;
        }

        protected override async Task<HttpResponseMessage> SendAsync( HttpRequestMessage request, CancellationToken cancellationToken ) {
            if( await mTokenRefresher.TokenRefreshRequired( 10 )) {
                await mTokenRefresher.RefreshToken();
            }

            var token = await mLocalStorage.GetItemAsync<string>( LocalStorageNames.AuthToken, cancellationToken );

            if(!String.IsNullOrWhiteSpace( token )) {
                request.Headers.Authorization = new AuthenticationHeaderValue( "bearer", token );
            }

            return await base.SendAsync( request, cancellationToken );
            /*
            if(( response.StatusCode == HttpStatusCode.Unauthorized ) ||
               ( response.StatusCode == HttpStatusCode.Forbidden )) {
                token = await RefreshTokenAsync();
                request.Headers.Authorization = new AuthenticationHeaderValue( token.Scheme, token.AccessToken );
                response = await base.SendAsync( request, cancellationToken );
            }
            */
        }
    }
}
