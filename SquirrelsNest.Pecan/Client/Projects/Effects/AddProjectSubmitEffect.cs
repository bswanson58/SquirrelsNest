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
    public class AddProjectSubmitEffect : Effect<AddProjectSubmitAction> {
        private readonly IAuthenticatedHttpHandler          mHttpHandler;
        private readonly ILogger<AddProjectSubmitEffect>    mLogger;

        public AddProjectSubmitEffect( ILogger<AddProjectSubmitEffect> logger, IAuthenticatedHttpHandler httpHandler ) {
            mLogger = logger;
            mHttpHandler = httpHandler;
        }

        public override async Task HandleAsync( AddProjectSubmitAction action, IDispatcher dispatcher ) {
            dispatcher.Dispatch( new ApiCallStarted( "Adding Project" ));

            try {
                var response = await mHttpHandler.Post<CreateProjectResponse>( CreateProjectRequest.Route, action.ProjectRequest );

                if(( response?.Project != null ) &&
                   ( response.Succeeded )) {
                    dispatcher.Dispatch( new AddProjectSuccess( response.Project ));
                }
                else {
                    dispatcher.Dispatch( new AddProjectFailure( response?.Message ?? "Received null response" ));
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
