using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.Extensions.Logging;
using SquirrelsNest.Pecan.Client.Constants;
using SquirrelsNest.Pecan.Client.Issues.Actions;
using SquirrelsNest.Pecan.Client.Ui.Actions;
using SquirrelsNest.Pecan.Shared.Dto.Issues;

namespace SquirrelsNest.Pecan.Client.Issues.Effects {
    // ReSharper disable once UnusedType.Global
    public class UpdateIssueSubmitEffect : Effect<UpdateIssueSubmit> {
        private readonly IHttpClientFactory                 mClientFactory;
        private readonly ILogger<UpdateIssueSubmitEffect>   mLogger;

        public UpdateIssueSubmitEffect( IHttpClientFactory clientFactory, ILogger<UpdateIssueSubmitEffect> logger ) {
            mClientFactory = clientFactory;
            mLogger = logger;
        }

        public override async Task HandleAsync( UpdateIssueSubmit action, IDispatcher dispatcher ) {
            dispatcher.Dispatch( new ApiCallStarted( "Updating Issue" ));

            try {
                using var httpClient = mClientFactory.CreateClient( HttpClientNames.Authenticated );
                var postResponse = await httpClient.PostAsJsonAsync( UpdateIssueRequest.Route, action.Request );
                var response = await postResponse.Content.ReadFromJsonAsync<UpdateIssueResponse>();

                if(( response?.Issue != null ) &&
                   ( response.Succeeded )) {
                    dispatcher.Dispatch( new UpdateIssueSuccess( response.Issue ));
                }
                else {
                    dispatcher.Dispatch( new UpdateIssueFailure( "Received null response" ));
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
