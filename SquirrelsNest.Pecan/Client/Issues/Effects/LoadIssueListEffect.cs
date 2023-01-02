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
    public class LoadIssueListEffect : Effect<LoadIssueListAction> {
        private readonly IHttpClientFactory             mClientFactory;
        private readonly ILogger<LoadIssueListEffect>   mLogger;

        public LoadIssueListEffect( IHttpClientFactory clientFactory, ILogger<LoadIssueListEffect> logger ) {
            mClientFactory = clientFactory;
            mLogger = logger;
        }

        public override async Task HandleAsync( LoadIssueListAction action, IDispatcher dispatcher ) {
            dispatcher.Dispatch( new ApiCallStarted( "Loading Issue List" ));

            try {
                using var httpClient = mClientFactory.CreateClient( HttpClientNames.Authenticated );
                var postResponse = await httpClient.PostAsJsonAsync( GetIssuesRequest.Route, 
                                                                     new GetIssuesRequest( action.Project.EntityId ));
                var response = await postResponse.Content.ReadFromJsonAsync<GetIssuesResponse>();

                if( response?.Succeeded == true ) {
                    dispatcher.Dispatch( new LoadIssueListSuccessAction( response.Issues ));
                }
                else {
                    dispatcher.Dispatch( new LoadIssueListFailureAction( "Received null response" ));
                }
            }
            catch( HttpRequestException exception ) {
                mLogger.LogError( exception, String.Empty );

                dispatcher.Dispatch( new LoadIssueListFailureAction( exception.Message ));
            }

            dispatcher.Dispatch( new ApiCallCompleted());
        }
    }
}
