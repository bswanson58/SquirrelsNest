using System;
using Fluxor;
using Microsoft.Extensions.Logging;
using SquirrelsNest.Pecan.Client.Constants;
using SquirrelsNest.Pecan.Client.Projects.Actions;
using SquirrelsNest.Pecan.Shared.Dto.Projects;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SquirrelsNest.Pecan.Client.Ui.Actions;

namespace SquirrelsNest.Pecan.Client.Projects.Effects {
    // ReSharper disable once UnusedType.Global
    public class IssueTypeChangeSubmitEffect : Effect<IssueTypeChangeSubmitAction> {
        private readonly IHttpClientFactory                     mClientFactory;
        private readonly ILogger<IssueTypeChangeSubmitEffect>   mLogger;

        public IssueTypeChangeSubmitEffect( IHttpClientFactory clientFactory, ILogger<IssueTypeChangeSubmitEffect> logger ) {
            mClientFactory = clientFactory;
            mLogger = logger;
        }

        public override async Task HandleAsync( IssueTypeChangeSubmitAction action, IDispatcher dispatcher ) {
            dispatcher.Dispatch( new ApiCallStarted( "Requesting Issue Type Change" ));

            try {
                using var httpClient = mClientFactory.CreateClient( HttpClientNames.Authenticated );
                var postResponse = await httpClient.PostAsJsonAsync( IssueTypeChangeInput.Route, action.Input );
                var response = await postResponse.Content.ReadFromJsonAsync<IssueTypeChangeResponse>();

                if(( response?.IssueType != null ) &&
                   ( response.Succeeded )) {
                    dispatcher.Dispatch( new IssueTypeChangeSuccessAction( response ));
                }
                else {
                    dispatcher.Dispatch( new IssueTypeChangeFailureAction( "Received null response" ));
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
