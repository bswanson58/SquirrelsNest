using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.Extensions.Logging;
using SquirrelsNest.Pecan.Client.Projects.Actions;
using SquirrelsNest.Pecan.Shared.Constants;
using SquirrelsNest.Pecan.Shared.Dto.Projects;

namespace SquirrelsNest.Pecan.Client.Projects.Effects {
    // ReSharper disable once ClassNeverInstantiated.Global
    public class GetProjectsEffect : Effect<GetProjectsAction> {
        private readonly    ILogger<GetProjectsEffect>  mLogger;
        private readonly    HttpClient                  mHttpClient;

        public GetProjectsEffect( HttpClient httpClient, ILogger<GetProjectsEffect> logger ) {
            mHttpClient = httpClient;
            mLogger = logger;
        }

        public override async Task HandleAsync( GetProjectsAction action, IDispatcher dispatcher ) {
            try {
                var response = await mHttpClient.GetFromJsonAsync<GetProjectsResponse>( Routes.GetProjects );

                if( response?.Succeeded == true ) {
                    dispatcher.Dispatch( new GetProjectsSuccessAction( response.Projects ) );
                }
                else {
                    dispatcher.Dispatch( new GetProjectsFailureAction( "Received null response" ) );
                }
            }
            catch( HttpRequestException exception ) {
                mLogger.LogError( exception, String.Empty );

                dispatcher.Dispatch( new GetProjectsFailureAction( exception.Message ) );
            }
        }
    }
}

