using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SquirrelsNest.Pecan.Client.Constants;
using SquirrelsNest.Pecan.Shared.Dto;

namespace SquirrelsNest.Pecan.Client.Support {
    public interface IAuthenticatedHttpHandler {
        Task<TResponse ?>   Get<TResponse>( string route );
        Task<TResponse ?>   Post<TResponse>( string route, object request );
    }

    public class AuthenticatedHttpHandler : IAuthenticatedHttpHandler {
        private readonly IHttpClientFactory     mClientFactory;
        private readonly IResponseStatusHandler mStatusHandler;

        public AuthenticatedHttpHandler( IHttpClientFactory clientFactory, IResponseStatusHandler statusHandler ) {
            mClientFactory = clientFactory;
            mStatusHandler = statusHandler;
        }

        public async Task<TResponse ?> Get<TResponse>( string route ) {
            try {
                using var httpClient = mClientFactory.CreateClient( HttpClientNames.Authenticated );

                return await HandleResponse<TResponse>( await httpClient.GetAsync( route ));
            }
            catch( Exception ex ) {
                mStatusHandler.HandleException( ex );

                return default;
            }
        }

        public async Task<TResponse ?> Post<TResponse>( string route, object request ) {
            try {
                using var httpClient = mClientFactory.CreateClient( HttpClientNames.Authenticated );

                return await HandleResponse<TResponse>( await httpClient.PostAsJsonAsync( route, request ));
            }
            catch( Exception ex ) {
                mStatusHandler.HandleException( ex );

                return default;
            }
        }

        private async Task<TResponse ?> HandleResponse<TResponse>( HttpResponseMessage response ) {
            if( response.StatusCode == HttpStatusCode.NoContent ) {
                return default;
            }

            if(!response.IsSuccessStatusCode ) {
                mStatusHandler.HandleStatusCode( response );

                return default;
            }

            var retValue = await response.Content.ReadFromJsonAsync<TResponse>();

            if(( retValue is BaseResponse message ) &&
               (!message.Succeeded )) {
                mStatusHandler.HandleCallFailure( message.Message );

                return default;
            }

            return retValue;
        }
    }
}
