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
    public class DeleteIssueSubmitEffect : Effect<DeleteIssueSubmitAction> {
        private readonly IAuthenticatedHttpHandler          mHttpHandler;
        private readonly ILogger<DeleteIssueSubmitEffect>   mLogger;

        public DeleteIssueSubmitEffect( IAuthenticatedHttpHandler httpHandler, ILogger<DeleteIssueSubmitEffect> logger ) {
            mHttpHandler = httpHandler;
            mLogger = logger;
        }

        public override async Task HandleAsync( DeleteIssueSubmitAction action, IDispatcher dispatcher ) {
            dispatcher.Dispatch( new ApiCallStarted( "Deleting Issue" ));

            try {
                var response = await mHttpHandler.Post<DeleteIssueResponse>( DeleteIssueRequest.Route, action.Request );

                if(( response?.Issue != null ) &&
                   ( response.Succeeded )) {
                    dispatcher.Dispatch( new DeleteIssueSuccess( response.Issue ));
                }
                else {
                    dispatcher.Dispatch( new DeleteIssueFailure( response?.Message ?? "Received null response" ));
                }
            }
            catch ( HttpRequestException exception ) {
                mLogger.LogError( exception, String.Empty );

                dispatcher.Dispatch( new DeleteIssueFailure( exception.Message ));
            }

            dispatcher.Dispatch( new ApiCallCompleted());
        }
    }
}
