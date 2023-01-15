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
    public class CreateProjectTemplateSubmitEffect : Effect<CreateProjectTemplateSubmit> {
        private readonly IAuthenticatedHttpHandler  mHttpHandler;
        private readonly ILogger<CreateProjectTemplateSubmitEffect> mLogger;

        public CreateProjectTemplateSubmitEffect( IAuthenticatedHttpHandler httpHandler,
                                                  ILogger<CreateProjectTemplateSubmitEffect> logger ) {
            mHttpHandler = httpHandler;
            mLogger = logger;
        }

        public override async Task HandleAsync( CreateProjectTemplateSubmit action, IDispatcher dispatcher ) {
            dispatcher.Dispatch( new ApiCallStarted( "Creating Project Template" ));

            try {
                var response = await mHttpHandler
                    .Post<CreateProjectTemplateResponse>( CreateProjectTemplateRequest.Route, action.Request );

                if(( response?.Template != null ) &&
                   ( response.Succeeded )) {
                    dispatcher.Dispatch( new CreateProjectTemplateSuccess( response.Template ));
                }
                else {
                    dispatcher.Dispatch( new CreateProjectTemplateFailure( response?.Message ?? "Received null response" ));
                }
            }
            catch ( HttpRequestException exception ) {
                mLogger.LogError( exception, String.Empty );

                dispatcher.Dispatch( new CreateProjectTemplateFailure( exception.Message ));
            }

            dispatcher.Dispatch( new ApiCallCompleted());
        }
    }
}
