using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.Extensions.Logging;
using SquirrelsNest.Pecan.Client.Constants;
using SquirrelsNest.Pecan.Client.Issues.Actions;
using SquirrelsNest.Pecan.Client.Projects.Actions;
using SquirrelsNest.Pecan.Client.Ui.Actions;
using SquirrelsNest.Pecan.Shared.Dto.Issues;

namespace SquirrelsNest.Pecan.Client.Issues.Effects {
    // ReSharper disable once UnusedType.Global
    public class AddIssueSubmitEffect : Effect<AddIssueSubmitAction> {
        private readonly IHttpClientFactory             mClientFactory;
        private readonly ILogger<AddIssueSubmitEffect>  mLogger;

        public AddIssueSubmitEffect( IHttpClientFactory clientFactory, ILogger<AddIssueSubmitEffect> logger ) {
            mClientFactory = clientFactory;
            mLogger = logger;
        }

        public override async Task HandleAsync( AddIssueSubmitAction action, IDispatcher dispatcher ) {
            dispatcher.Dispatch( new ApiCallStarted( "Creating New Issue" ));

            try {
                using var httpClient = mClientFactory.CreateClient( HttpClientNames.Authenticated );
                var postResponse = await httpClient.PostAsJsonAsync( CreateIssueRequest.Route, action.Request );
                var response = await postResponse.Content.ReadFromJsonAsync<CreateIssueResponse>();

                if(( response?.Issue != null ) &&
                   ( response.Succeeded )) {
                    dispatcher.Dispatch( new AddIssueSuccess( response.Issue ));
                }
                else {
                    dispatcher.Dispatch( new AddIssueFailure( "Received null response" ));
                }
            }
            catch ( HttpRequestException exception ) {
                mLogger.LogError( exception, String.Empty );

                dispatcher.Dispatch( new AddProjectFailure( exception.Message ));
            }

            dispatcher.Dispatch( new ApiCallCompleted());
        }
    }
}
