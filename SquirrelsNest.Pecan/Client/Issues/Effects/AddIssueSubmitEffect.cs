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
    public class AddIssueSubmitEffect : Effect<AddIssueSubmitAction> {
        private readonly IAuthenticatedHttpHandler      mHttpHandler;
        private readonly ILogger<AddIssueSubmitEffect>  mLogger;

        public AddIssueSubmitEffect( IAuthenticatedHttpHandler httpHandler, ILogger<AddIssueSubmitEffect> logger ) {
            mHttpHandler = httpHandler;
            mLogger = logger;
        }

        public override async Task HandleAsync( AddIssueSubmitAction action, IDispatcher dispatcher ) {
            dispatcher.Dispatch( new ApiCallStarted( "Creating New Issue" ));

            try {
                var response = await mHttpHandler.Post<CreateIssueResponse>( CreateIssueRequest.Route, action.Request );

                if(( response?.Issue != null ) &&
                   ( response.Succeeded )) {
                    dispatcher.Dispatch( new AddIssueSuccess( response.Issue ));
                }
                else {
                    dispatcher.Dispatch( new AddIssueFailure( response?.Message ?? "Received null response" ));
                }
            }
            catch ( HttpRequestException exception ) {
                mLogger.LogError( exception, String.Empty );

                dispatcher.Dispatch( new AddIssueFailure( exception.Message ));
            }

            dispatcher.Dispatch( new ApiCallCompleted());
        }
    }
}
