using System;
using System.Net.Http;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.Extensions.Logging;
using SquirrelsNest.Pecan.Client.Projects.Actions;
using SquirrelsNest.Pecan.Client.Support;
using SquirrelsNest.Pecan.Client.Ui.Actions;
using SquirrelsNest.Pecan.Shared.Dto.ProjectTemplates;

namespace SquirrelsNest.Pecan.Client.Projects.Effects {
    // ReSharper disable once UnusedType.Global
    public class AddProjectFromTemplateSubmitEffect : Effect<AddProjectFromTemplateSubmitAction> {
        private readonly IAuthenticatedHttpHandler          mHttpHandler;
        private readonly ILogger<AddProjectSubmitEffect>    mLogger;

        public AddProjectFromTemplateSubmitEffect( IAuthenticatedHttpHandler httpHandler, ILogger<AddProjectSubmitEffect> logger ) {
            mHttpHandler = httpHandler;
            mLogger = logger;
        }

        public override async Task HandleAsync( AddProjectFromTemplateSubmitAction action, IDispatcher dispatcher ) {
            dispatcher.Dispatch( new ApiCallStarted( "Adding Project" ));

            try {
                var request = new CreateProjectFromTemplateRequest(
                    action.ProjectRequest.ProjectTemplateName, 
                    action.ProjectRequest.Name, 
                    action.ProjectRequest.Description );
                var response = await mHttpHandler.Post<CreateProjectFromTemplateResponse>( CreateProjectFromTemplateRequest.Route, request );

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
