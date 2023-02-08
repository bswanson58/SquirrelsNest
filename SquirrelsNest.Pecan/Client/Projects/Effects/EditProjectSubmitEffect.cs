using System;
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
    public class EditProjectSubmitEffect : Effect<EditProjectSubmit> {
        private readonly IAuthenticatedHttpHandler          mHttpHandler;
        private readonly ILogger<EditProjectSubmitEffect>   mLogger;

        public EditProjectSubmitEffect( IAuthenticatedHttpHandler httpHandler, ILogger<EditProjectSubmitEffect> logger ) {
            mHttpHandler = httpHandler;
            mLogger = logger;
        }

        public override async Task HandleAsync( EditProjectSubmit action, IDispatcher dispatcher ) {
            dispatcher.Dispatch( new ApiCallStarted( "Updating Project" ));

            try {
                var response = await mHttpHandler.Post<EditProjectResponse>( EditProjectRequest.Route, action.Request );

                if(( response?.Project != null ) &&
                   ( response.Succeeded )) {
                    dispatcher.Dispatch( new EditProjectSuccess( response.Project ));
                }
                else {
                    dispatcher.Dispatch( new EditProjectFailure( response?.Message ?? "Received null response" ));
                }
            }
            catch ( HttpRequestException exception ) {
                mLogger.LogError( exception, String.Empty );

                dispatcher.Dispatch( new EditProjectFailure( exception.Message ));
            }

            dispatcher.Dispatch( new ApiCallCompleted());
        }
    }
}
