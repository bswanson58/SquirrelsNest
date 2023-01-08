using System;
using System.Net.Http;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.Extensions.Logging;
using SquirrelsNest.Pecan.Client.Projects.Actions;
using SquirrelsNest.Pecan.Client.Support;
using SquirrelsNest.Pecan.Client.Ui.Actions;
using SquirrelsNest.Pecan.Shared.Constants;
using SquirrelsNest.Pecan.Shared.Dto.Projects;

namespace SquirrelsNest.Pecan.Client.Projects.Effects {
    // ReSharper disable once ClassNeverInstantiated.Global
    public class GetProjectsEffect : Effect<GetProjectsAction> {
        private readonly IAuthenticatedHttpHandler      mHttpHandler;
        private readonly ILogger<GetProjectsEffect>     mLogger;

        public GetProjectsEffect( ILogger<GetProjectsEffect> logger, IAuthenticatedHttpHandler httpHandler ) {
            mLogger = logger;
            mHttpHandler = httpHandler;
        }

        public override async Task HandleAsync( GetProjectsAction action, IDispatcher dispatcher ) {
            dispatcher.Dispatch( new ApiCallStarted( "Loading Project List" ));

            try {
                var response = await mHttpHandler.Get<GetProjectsResponse>( Routes.GetProjects );

                if( response?.Succeeded == true ) {
                    dispatcher.Dispatch( new GetProjectsSuccessAction( response.Projects ));
                }
                else {
                    dispatcher.Dispatch( new GetProjectsFailureAction( response?.Message ?? "Received null response" ));
                }
            }
            catch( HttpRequestException exception ) {
                mLogger.LogError( exception, String.Empty );

                dispatcher.Dispatch( new GetProjectsFailureAction( exception.Message ));
            }

            dispatcher.Dispatch( new ApiCallCompleted());
        }
    }
}

