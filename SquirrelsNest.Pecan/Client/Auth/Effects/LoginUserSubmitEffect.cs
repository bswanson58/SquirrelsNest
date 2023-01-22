using System;
using Fluxor;
using Microsoft.Extensions.Logging;
using SquirrelsNest.Pecan.Client.Auth.Actions;
using SquirrelsNest.Pecan.Shared.Dto.Auth;
using System.Net.Http;
using System.Threading.Tasks;
using SquirrelsNest.Pecan.Client.Support;
using SquirrelsNest.Pecan.Client.Ui.Actions;

namespace SquirrelsNest.Pecan.Client.Auth.Effects {
    // ReSharper disable once UnusedType.Global
    public class LoginUserSubmitEffect : Effect<LoginUserSubmitAction> {
        private readonly ILogger<LoginUserSubmitEffect> mLogger;
        private readonly IAnonymousHttpHandler          mHttpHandler;

        public LoginUserSubmitEffect( IAnonymousHttpHandler httpHandler, ILogger<LoginUserSubmitEffect> logger ) {
            mHttpHandler = httpHandler;
            mLogger = logger;
        }

        public override async Task HandleAsync( LoginUserSubmitAction action, IDispatcher dispatcher ) {
            dispatcher.Dispatch( new ApiCallStarted( "Requesting User Authentication" ));

            try {
                var response = await mHttpHandler.Post<LoginUserResponse>( LoginUserInput.Route, action.UserInput  );

                if( response != null ) {
                    if( response.Succeeded ) {
                        dispatcher.Dispatch( new LoginUserSuccessAction( response ));
                    }
                    else {
                        dispatcher.Dispatch( new LoginUserFailureAction( response.Message ));
                    }
                }
                else {
                    dispatcher.Dispatch( new LoginUserFailureAction( response?.Message ?? "Received null response" ));
                }
            }
            catch( HttpRequestException exception ) {
                mLogger.LogError( exception, String.Empty );

                dispatcher.Dispatch( new LoginUserFailureAction( exception.Message ));
            }

            dispatcher.Dispatch( new ApiCallCompleted());
        }
    }
}
