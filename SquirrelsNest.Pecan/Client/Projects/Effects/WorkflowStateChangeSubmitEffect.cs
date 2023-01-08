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
    public class WorkflowStateChangeSubmitEffect : Effect<WorkflowStateChangeSubmitAction> {
        private readonly IAuthenticatedHttpHandler                  mHttpHandler;
        private readonly ILogger<WorkflowStateChangeSubmitEffect>   mLogger;

        public WorkflowStateChangeSubmitEffect( IAuthenticatedHttpHandler httpHandler, ILogger<WorkflowStateChangeSubmitEffect> logger ) {
            mHttpHandler = httpHandler;
            mLogger = logger;
        }

        public override async Task HandleAsync( WorkflowStateChangeSubmitAction action, IDispatcher dispatcher ) {
            dispatcher.Dispatch( new ApiCallStarted( "Requesting Workflow State Change" ));

            try {
                var response = await mHttpHandler.Post<WorkflowStateChangeResponse>( WorkflowStateChangeInput.Route, action.Input );

                if(( response?.WorkflowState != null ) &&
                   ( response.Succeeded )) {
                    dispatcher.Dispatch( new WorkflowStateChangeSuccessAction( response ));
                }
                else {
                    dispatcher.Dispatch( new WorkflowStateChangeFailureAction( response?.Message ?? "Received null response" ));
                }
            }
            catch ( HttpRequestException exception ) {
                mLogger.LogError( exception, String.Empty );

                dispatcher.Dispatch( new WorkflowStateChangeFailureAction( exception.Message ));
            }

            dispatcher.Dispatch( new ApiCallCompleted());
        }
    }
}
