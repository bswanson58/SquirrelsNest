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
    public class ComponentChangeSubmitEffect : Effect<ComponentChangeSubmitAction> {
        private readonly IAuthenticatedHttpHandler              mHttpHandler;
        private readonly ILogger<ComponentChangeSubmitEffect>   mLogger;

        public ComponentChangeSubmitEffect( IAuthenticatedHttpHandler httpHandler, ILogger<ComponentChangeSubmitEffect> logger ) {
            mHttpHandler = httpHandler;
            mLogger = logger;
        }

        public override async Task HandleAsync( ComponentChangeSubmitAction action, IDispatcher dispatcher ) {
            dispatcher.Dispatch( new ApiCallStarted( "Requesting Component Change" ));

            try {
                var response = await mHttpHandler.Post<ComponentChangeResponse>( ComponentChangeInput.Route, action.Input );

                if(( response?.Component != null ) &&
                   ( response.Succeeded )) {
                    dispatcher.Dispatch( new ComponentChangeSuccessAction( response ));
                }
                else {
                    dispatcher.Dispatch( new ComponentChangeFailureAction( response?.Message ?? "Received null response" ));
                }
            }
            catch ( HttpRequestException exception ) {
                mLogger.LogError( exception, String.Empty );

                dispatcher.Dispatch( new ComponentChangeFailureAction( exception.Message ));
            }

            dispatcher.Dispatch( new ApiCallCompleted());
        }
    }
}
