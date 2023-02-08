using System;
using System.Net.Http;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.Extensions.Logging;
using SquirrelsNest.Pecan.Client.Auth.Actions;
using SquirrelsNest.Pecan.Client.Support;
using SquirrelsNest.Pecan.Client.Ui.Actions;
using SquirrelsNest.Pecan.Shared.Dto.Auth;

namespace SquirrelsNest.Pecan.Client.Auth.Effects {
    // ReSharper disable once UnusedType.Global
    public class ChangeUserRolesSubmitEffect : Effect<ChangeUserRolesSubmit> {
        private readonly ILogger<ChangeUserRolesSubmitEffect>   mLogger;
        private readonly IAuthenticatedHttpHandler              mHttpHandler;

        public ChangeUserRolesSubmitEffect( IAuthenticatedHttpHandler httpHandler, ILogger<ChangeUserRolesSubmitEffect> logger ) {
            mHttpHandler = httpHandler;
            mLogger = logger;
        }

        public override async Task HandleAsync( ChangeUserRolesSubmit action, IDispatcher dispatcher ) {
            dispatcher.Dispatch( new ApiCallStarted( "Updating User Roles" ));

            try {
                var response = await mHttpHandler.Post<ChangeUserRolesResponse>( ChangeUserRolesRequest.Route, action.Request );

                if(( response?.User != null ) &&
                   ( response.Succeeded )) {
                    dispatcher.Dispatch( new ChangeUserRolesSuccess( response.User ));
                }
                else {
                    dispatcher.Dispatch( new ChangeUserRolesFailure( response?.Message ?? "Received null response" ));
                }
            }
            catch ( HttpRequestException exception ) {
                mLogger.LogError( exception, String.Empty );

                dispatcher.Dispatch( new ChangeUserRolesFailure( exception.Message ));
            }

            dispatcher.Dispatch( new ApiCallCompleted());
        }
    }
}
