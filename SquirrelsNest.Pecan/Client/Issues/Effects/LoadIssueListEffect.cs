using System;
using System.Net.Http;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.Extensions.Logging;
using SquirrelsNest.Pecan.Client.Issues.Actions;
using SquirrelsNest.Pecan.Client.Support;
using SquirrelsNest.Pecan.Client.Ui.Actions;
using SquirrelsNest.Pecan.Shared.Dto.Issues;

namespace SquirrelsNest.Pecan.Client.Issues.Effects {
    // ReSharper disable once UnusedType.Global
    public class LoadIssueListEffect : Effect<LoadIssueListAction> {
        private readonly IAuthenticatedHttpHandler      mHttpHandler;
        private readonly ILogger<LoadIssueListEffect>   mLogger;

        public LoadIssueListEffect( IAuthenticatedHttpHandler httpHandler, ILogger<LoadIssueListEffect> logger ) {
            mHttpHandler = httpHandler;
            mLogger = logger;
        }

        public override async Task HandleAsync( LoadIssueListAction action, IDispatcher dispatcher ) {
            dispatcher.Dispatch( new ApiCallStarted( "Loading Issue List" ));

            try {
                var request = new GetIssuesRequest( action.Project.EntityId, action.PageRequest );
                var response = await mHttpHandler.Post<GetIssuesResponse>( GetIssuesRequest.Route, request );

                if( response?.Succeeded == true ) {
                    dispatcher.Dispatch( new LoadIssueListSuccessAction( response.Issues, response.PageInformation ));
                }
                else {
                    dispatcher.Dispatch( new LoadIssueListFailureAction( response?.Message ?? "Received null response" ));
                }
            }
            catch( HttpRequestException exception ) {
                mLogger.LogError( exception, String.Empty );

                dispatcher.Dispatch( new LoadIssueListFailureAction( exception.Message ));
            }

            dispatcher.Dispatch( new ApiCallCompleted());
        }
    }
}
