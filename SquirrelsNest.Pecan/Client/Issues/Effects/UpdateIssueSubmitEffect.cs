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
    public class UpdateIssueSubmitEffect : Effect<UpdateIssueSubmit> {
        private readonly IAuthenticatedHttpHandler          mHttpHandler;
        private readonly ILogger<UpdateIssueSubmitEffect>   mLogger;

        public UpdateIssueSubmitEffect( IAuthenticatedHttpHandler httpHandler, ILogger<UpdateIssueSubmitEffect> logger ) {
            mHttpHandler = httpHandler;
            mLogger = logger;
        }

        public override async Task HandleAsync( UpdateIssueSubmit action, IDispatcher dispatcher ) {
            dispatcher.Dispatch( new ApiCallStarted( "Updating Issue" ));

            try {
                var response = await mHttpHandler.Post<UpdateIssueResponse>( UpdateIssueRequest.Route, action.Request );

                if(( response?.Issue != null ) &&
                   ( response.Succeeded )) {
                    dispatcher.Dispatch( new UpdateIssueSuccess( response.Issue ));
                }
                else {
                    dispatcher.Dispatch( new UpdateIssueFailure( response?.Message ?? "Received null response" ));
                }
            }
            catch ( HttpRequestException exception ) {
                mLogger.LogError( exception, String.Empty );

                dispatcher.Dispatch( new UpdateIssueFailure( exception.Message ));
            }

            dispatcher.Dispatch( new ApiCallCompleted());
        }
    }
}
