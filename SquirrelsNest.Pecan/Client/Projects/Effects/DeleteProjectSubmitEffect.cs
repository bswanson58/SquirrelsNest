using System.Net.Http;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.Extensions.Logging;
using SquirrelsNest.Pecan.Client.Projects.Actions;
using SquirrelsNest.Pecan.Client.Support;
using SquirrelsNest.Pecan.Client.Ui.Actions;
using SquirrelsNest.Pecan.Shared.Dto.Projects;

namespace SquirrelsNest.Pecan.Client.Projects.Effects {
    // ReSharper disable once UnusedType.Global
    public class DeleteProjectSubmitEffect : Effect<DeleteProjectSubmit> {
        private readonly IAuthenticatedHttpHandler mHttpHandler;
        private readonly ILogger<DeleteProjectSubmitEffect> mLogger;

        public DeleteProjectSubmitEffect( IAuthenticatedHttpHandler httpHandler, ILogger<DeleteProjectSubmitEffect> logger ) {
            mHttpHandler = httpHandler;
            mLogger = logger;
        }

        public override async Task HandleAsync( DeleteProjectSubmit action, IDispatcher dispatcher ) {
            dispatcher.Dispatch( new ApiCallStarted( "Deleting Project" ));

            try {
                var response = await mHttpHandler.Post<DeleteProjectResponse>( DeleteProjectRequest.Route, action.Request );

                if( response?.Project != null &&
                   response.Succeeded ) {
                    dispatcher.Dispatch( new DeleteProjectSuccess( response.Project ));
                }
                else {
                    dispatcher.Dispatch( new DeleteProjectFailure( response?.Message ?? "Received null response" ));
                }
            }
            catch( HttpRequestException exception ) {
                mLogger.LogError( exception, string.Empty );

                dispatcher.Dispatch( new DeleteProjectFailure( exception.Message ));
            }

            dispatcher.Dispatch( new ApiCallCompleted());
        }
    }
}
