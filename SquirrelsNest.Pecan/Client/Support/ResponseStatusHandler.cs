using System;
using System.Net;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using SquirrelsNest.Pecan.Client.Constants;
using SquirrelsNest.Pecan.Client.Ui;

namespace SquirrelsNest.Pecan.Client.Support {
    public interface IResponseStatusHandler {
        void    HandleStatusCode( HttpStatusCode statusCode );
        void    HandleCallFailure( string message );
        void    HandleException( Exception ex );
    }

    public class ResponseStatusHandler : IResponseStatusHandler {
        private readonly NavigationManager                  mNavigationManager;
        private readonly UiFacade                           mUiFacade;
        private readonly ILogger<ResponseStatusHandler>     mLogger;

        public ResponseStatusHandler( NavigationManager navigationManager,
                                      ILogger<ResponseStatusHandler> logger, UiFacade uiFacade ) {
            mLogger = logger;
            mUiFacade = uiFacade;
            mNavigationManager = navigationManager;
        }

        public void HandleStatusCode( HttpStatusCode statusCode ) {
            if( statusCode is 
               HttpStatusCode.NoContent or 
               HttpStatusCode.Accepted or 
               HttpStatusCode.OK ) {
                return;
            }

            if( statusCode is HttpStatusCode.Unauthorized ) {
                mUiFacade.ApiCallFailure( "Unauthorized request, please login." );

                mNavigationManager.NavigateTo( LocalRouteNames.Login );
            }
        }

        public void HandleCallFailure( string message ) {
            mUiFacade.ApiCallFailure( message );
        }

        public void HandleException( Exception ex ) {
            mLogger.LogError( ex, String.Empty );

            mUiFacade.ApiCallFailure( $"Error: {ex.Message}" );
        }
    }
}
