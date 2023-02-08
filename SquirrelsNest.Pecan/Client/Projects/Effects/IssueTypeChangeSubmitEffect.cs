using System;
using Fluxor;
using Microsoft.Extensions.Logging;
using SquirrelsNest.Pecan.Client.Projects.Actions;
using SquirrelsNest.Pecan.Shared.Dto.Projects;
using System.Net.Http;
using System.Threading.Tasks;
using SquirrelsNest.Pecan.Client.Support;
using SquirrelsNest.Pecan.Client.Ui.Actions;

namespace SquirrelsNest.Pecan.Client.Projects.Effects {
    // ReSharper disable once UnusedType.Global
    public class IssueTypeChangeSubmitEffect : Effect<IssueTypeChangeSubmitAction> {
        private readonly IAuthenticatedHttpHandler              mHttpHandler;
        private readonly ILogger<IssueTypeChangeSubmitEffect>   mLogger;

        public IssueTypeChangeSubmitEffect( IAuthenticatedHttpHandler httpHandler, ILogger<IssueTypeChangeSubmitEffect> logger ) {
            mHttpHandler = httpHandler;
            mLogger = logger;
        }

        public override async Task HandleAsync( IssueTypeChangeSubmitAction action, IDispatcher dispatcher ) {
            dispatcher.Dispatch( new ApiCallStarted( "Requesting Issue Type Change" ));

            try {
                var response = await mHttpHandler.Post<IssueTypeChangeResponse>( IssueTypeChangeInput.Route, action.Input );

                if(( response?.IssueType != null ) &&
                   ( response.Succeeded )) {
                    dispatcher.Dispatch( new IssueTypeChangeSuccessAction( response ));
                }
                else {
                    dispatcher.Dispatch( new IssueTypeChangeFailureAction( response?.Message ?? "Received null response" ));
                }
            }
            catch ( HttpRequestException exception ) {
                mLogger.LogError( exception, String.Empty );

                dispatcher.Dispatch( new IssueTypeChangeFailureAction( exception.Message ));
            }

            dispatcher.Dispatch( new ApiCallCompleted());
        }
    }
}
