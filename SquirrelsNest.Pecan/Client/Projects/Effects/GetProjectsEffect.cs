using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.Extensions.Logging;
using SquirrelsNest.Pecan.Client.Projects.Actions;
using SquirrelsNest.Pecan.Shared.Constants;
using SquirrelsNest.Pecan.Shared.Dto;

namespace SquirrelsNest.Pecan.Client.Projects.Effects {
    public class GetProjectsEffect : Effect<GetProjectsAction> {
        private readonly    ILogger<GetProjectsEffect>  mLogger;
        private readonly    HttpClient                  mHttpClient;

        public GetProjectsEffect( HttpClient httpClient, ILogger<GetProjectsEffect> logger ) {
            mHttpClient = httpClient;
            mLogger = logger;
        }

        public override async Task HandleAsync( GetProjectsAction action, IDispatcher dispatcher ) {
            try {
                mLogger.LogInformation( "GetProjectsEffect" );
                
                var response = await mHttpClient.GetFromJsonAsync<GetProjectsResponse>( Routes.GetProjects );

                mLogger.LogInformation( "GetProjectsEffect success" );

                dispatcher.Dispatch( new GetProjectsSuccessAction( response.Projects ));
            }
            catch ( Exception exception ) {
                mLogger.LogError( $"GetProjectsEffect: {exception.Message}" );

                dispatcher.Dispatch( new GetProjectsFailureAction( exception.Message ));
            }
        }
    }
}
