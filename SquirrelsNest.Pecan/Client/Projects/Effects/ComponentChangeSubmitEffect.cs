using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.Extensions.Logging;
using SquirrelsNest.Pecan.Client.Constants;
using SquirrelsNest.Pecan.Client.Projects.Actions;
using SquirrelsNest.Pecan.Client.Ui.Actions;
using SquirrelsNest.Pecan.Shared.Dto.Projects;

namespace SquirrelsNest.Pecan.Client.Projects.Effects {
    // ReSharper disable once UnusedType.Global
    public class ComponentChangeSubmitEffect : Effect<ComponentChangeSubmitAction> {
        private readonly IHttpClientFactory                     mClientFactory;
        private readonly ILogger<ComponentChangeSubmitEffect>   mLogger;

        public ComponentChangeSubmitEffect( IHttpClientFactory clientFactory, ILogger<ComponentChangeSubmitEffect> logger ) {
            mClientFactory = clientFactory;
            mLogger = logger;
        }

        public override async Task HandleAsync( ComponentChangeSubmitAction action, IDispatcher dispatcher ) {
            dispatcher.Dispatch( new ApiCallStarted( "Requesting Component Change" ));

            try {
                using var httpClient = mClientFactory.CreateClient( HttpClientNames.Authenticated );
                var postResponse = await httpClient.PostAsJsonAsync( ComponentChangeInput.Route, action.Input );
                var response = await postResponse.Content.ReadFromJsonAsync<ComponentChangeResponse>();

                if(( response?.Component != null ) &&
                   ( response.Succeeded )) {
                    dispatcher.Dispatch( new ComponentChangeSuccessAction( response ));
                }
                else {
                    dispatcher.Dispatch( new ComponentChangeFailureAction( "Received null response" ));
                }
            }
            catch ( HttpRequestException exception ) {
                mLogger.LogError( exception, String.Empty );

                dispatcher.Dispatch( new ComponentChangeFailureAction( exception.Message ));
            }

            dispatcher.Dispatch( new ApiCallCompleted());
        }
    }
}
