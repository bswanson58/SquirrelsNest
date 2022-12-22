﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Blazored.LocalStorage;

namespace SquirrelsNest.Pecan.Client.Auth.Support {
    public class JwtTokenHandler : DelegatingHandler {
        private readonly ILocalStorageService   mLocalStorage;

        public JwtTokenHandler( ILocalStorageService storageService ) {
            mLocalStorage = storageService;
        }

        protected override async Task<HttpResponseMessage> SendAsync( HttpRequestMessage request, CancellationToken cancellationToken ) {
            var token = await mLocalStorage.GetItemAsync<string>( "authToken", cancellationToken );

            if( !String.IsNullOrWhiteSpace( token ) ) {
                request.Headers.Authorization = new AuthenticationHeaderValue( "bearer", token );
            }

            var response = await base.SendAsync( request, cancellationToken );
            /*
                        if(( response.StatusCode == HttpStatusCode.Unauthorized ) ||
                           ( response.StatusCode == HttpStatusCode.Forbidden )) {
                            token = await RefreshTokenAsync();
                            request.Headers.Authorization = new AuthenticationHeaderValue( token.Scheme, token.AccessToken );
                            response = await base.SendAsync( request, cancellationToken );
                        }
            */
            return response;
        }
    }
}