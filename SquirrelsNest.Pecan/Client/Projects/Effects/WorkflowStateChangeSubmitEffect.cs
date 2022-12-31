using System;
using Fluxor;
using Microsoft.Extensions.Logging;
using SquirrelsNest.Pecan.Client.Constants;
using SquirrelsNest.Pecan.Client.Projects.Actions;
using SquirrelsNest.Pecan.Shared.Dto.Projects;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SquirrelsNest.Pecan.Client.Shared.Actions;

namespace SquirrelsNest.Pecan.Client.Projects.Effects {
    // ReSharper disable once UnusedType.Global
    public class WorkflowStateChangeSubmitEffect : Effect<WorkflowStateChangeSubmitAction> {
        private readonly IHttpClientFactory                         mClientFactory;
        private readonly ILogger<WorkflowStateChangeSubmitEffect>   mLogger;

        public WorkflowStateChangeSubmitEffect( IHttpClientFactory clientFactory, ILogger<WorkflowStateChangeSubmitEffect> logger ) {
            mClientFactory = clientFactory;
            mLogger = logger;
        }

        public override async Task HandleAsync( WorkflowStateChangeSubmitAction action, IDispatcher dispatcher ) {
            dispatcher.Dispatch( new ApiCallStarted( "Requesting Workflow State Change" ));

            try {
                using var httpClient = mClientFactory.CreateClient( HttpClientNames.Authenticated );
                var postResponse = await httpClient.PostAsJsonAsync( WorkflowStateChangeInput.Route, action.Input );
                var response = await postResponse.Content.ReadFromJsonAsync<WorkflowStateChangeResponse>();

                if(( response?.WorkflowState != null ) &&
                   ( response.Succeeded )) {
                    dispatcher.Dispatch( new WorkflowStateChangeSuccessAction( response ));
                }
                else {
                    dispatcher.Dispatch( new WorkflowStateChangeFailureAction( "Received null response" ));
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
