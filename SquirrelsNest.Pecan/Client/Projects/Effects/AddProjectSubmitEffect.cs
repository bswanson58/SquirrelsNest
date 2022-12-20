using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.Extensions.Logging;
using SquirrelsNest.Pecan.Client.Projects.Actions;
using SquirrelsNest.Pecan.Shared.Dto.Projects;

namespace SquirrelsNest.Pecan.Client.Projects.Effects {
    // ReSharper disable once UnusedType.Global
    public class AddProjectSubmitEffect : Effect<AddProjectSubmitAction> {
        private readonly    ILogger<AddProjectSubmitEffect> mLogger;
        private readonly    HttpClient                      mHttpClient;

        public AddProjectSubmitEffect( HttpClient httpClient, ILogger<AddProjectSubmitEffect> logger ) {
            mHttpClient = httpClient;
            mLogger = logger;
        }

        public override async Task HandleAsync( AddProjectSubmitAction action, IDispatcher dispatcher ) {
            try {
                var postResponse = await mHttpClient.PostAsJsonAsync( CreateProjectInput.Route, action.ProjectInput );
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
        }
    }
}
