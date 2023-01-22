using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SquirrelsNest.Pecan.Client.Constants;
using SquirrelsNest.Pecan.Shared.Dto;

namespace SquirrelsNest.Pecan.Client.Support {
    public interface IAuthenticatedHttpHandler : IBaseHttpHandler { }
    public interface IAnonymousHttpHandler: IBaseHttpHandler { }

    public interface IBaseHttpHandler {
        Task<TResponse ?>           Get<TResponse>( string route );
        Task<TResponse ?>           Post<TResponse>( string route, object request );
        Task<HttpResponseMessage ?> Post( string route, object request );
    }

    public class AuthenticatedHttpHandler : BaseHttpHandler, IAuthenticatedHttpHandler {
        public AuthenticatedHttpHandler( IHttpClientFactory clientFactory, IResponseStatusHandler statusHandler )
            : base( clientFactory, statusHandler ) { }

        protected override string   ClientName => HttpClientNames.Authenticated;
    }

    public class AnonymousHttpHandler : BaseHttpHandler, IAnonymousHttpHandler {
        public AnonymousHttpHandler( IHttpClientFactory clientFactory, IResponseStatusHandler statusHandler )
            : base( clientFactory, statusHandler ) { }

        protected override string   ClientName => HttpClientNames.Anonymous;
    }

    public abstract class BaseHttpHandler : IBaseHttpHandler {
        private readonly IHttpClientFactory     mClientFactory;
        private readonly IResponseStatusHandler mStatusHandler;

        protected abstract string               ClientName { get; }

        protected BaseHttpHandler( IHttpClientFactory clientFactory, IResponseStatusHandler statusHandler ) {
            mClientFactory = clientFactory;
            mStatusHandler = statusHandler;
        }

        public async Task<TResponse ?> Get<TResponse>( string route ) {
            try {
                using var httpClient = mClientFactory.CreateClient( ClientName );

                return await HandleResponse<TResponse>( await httpClient.GetAsync( route ));
            }
            catch( Exception ex ) {
                mStatusHandler.HandleException( ex );

                return default;
            }
        }

        public async Task<TResponse ?> Post<TResponse>( string route, object request ) {
            try {
                using var httpClient = mClientFactory.CreateClient( ClientName );

                return await HandleResponse<TResponse>( await httpClient.PostAsJsonAsync( route, request ));
            }
            catch( Exception ex ) {
                mStatusHandler.HandleException( ex );

                return default;
            }
        }

        public async Task<HttpResponseMessage ?> Post( string route, object request ) {
            try {
                using var httpClient = mClientFactory.CreateClient( ClientName );

                var response = await httpClient.PostAsJsonAsync( route, request );

                if(!response.IsSuccessStatusCode ) {
                    mStatusHandler.HandleStatusCode( response );
                }

                return response;
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
