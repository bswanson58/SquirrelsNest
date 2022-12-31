using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.Extensions.Logging;
using SquirrelsNest.Pecan.Client.Constants;
using SquirrelsNest.Pecan.Client.Projects.Actions;
using SquirrelsNest.Pecan.Client.Shared.Actions;
using SquirrelsNest.Pecan.Shared.Dto.Projects;

namespace SquirrelsNest.Pecan.Client.Projects.Effects {
    // ReSharper disable once UnusedType.Global
    public class AddProjectSubmitEffect : Effect<AddProjectSubmitAction> {
        private readonly IHttpClientFactory                 mClientFactory;
        private readonly ILogger<AddProjectSubmitEffect>    mLogger;

        public AddProjectSubmitEffect( IHttpClientFactory clientFactory,  ILogger<AddProjectSubmitEffect> logger ) {
            mClientFactory = clientFactory;
            mLogger = logger;
        }

        public override async Task HandleAsync( AddProjectSubmitAction action, IDispatcher dispatcher ) {
            dispatcher.Dispatch( new ApiCallStarted( "Adding Project" ));

            try {
                using var httpClient = mClientFactory.CreateClient( HttpClientNames.Authenticated );
                var postResponse = await httpClient.PostAsJsonAsync( CreateProjectInput.Route, action.ProjectInput );
                var response = await postResponse.Content.ReadFromJsonAsync<CreateProjectResponse>();

                if(( response?.Project != null ) &&
                   ( response.Succeeded )) {
                    dispatcher.Dispatch( new AddProjectSuccess( response.Project ));
                }
                else {
                    dispatcher.Dispatch( new AddProjectFailure( "Received null response" ));
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
