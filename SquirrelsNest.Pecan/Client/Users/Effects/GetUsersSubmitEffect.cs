using System;
using System.Net.Http;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.Extensions.Logging;
using SquirrelsNest.Pecan.Client.Support;
using SquirrelsNest.Pecan.Client.Ui.Actions;
using SquirrelsNest.Pecan.Client.Users.Actions;
using SquirrelsNest.Pecan.Shared.Dto.Users;

namespace SquirrelsNest.Pecan.Client.Users.Effects {
    // ReSharper disable once UnusedType.Global
    public class GetUsersSubmitEffect : Effect<GetUsersAction> {
        private readonly IAuthenticatedHttpHandler      mHttpHandler;
        private readonly ILogger<GetUsersSubmitEffect>  mLogger;

        public GetUsersSubmitEffect( IAuthenticatedHttpHandler httpHandler, ILogger<GetUsersSubmitEffect> logger ) {
            mHttpHandler = httpHandler;
            mLogger = logger;
        }

        public override async Task HandleAsync( GetUsersAction action, IDispatcher dispatcher ) {
            dispatcher.Dispatch( new ApiCallStarted( "Loading User List" ));

            try {
                var request = new GetUsersRequest( action.PageRequest );
                var response = await mHttpHandler.Post<GetUsersResponse>( GetUsersRequest.Route, request );

                if( response?.Succeeded == true ) {
                    dispatcher.Dispatch( new GetUsersSuccessAction( response.Users, response.PageInformation ));
                }
                else {
                    dispatcher.Dispatch( new GetUsersFailureAction( response?.Message ?? "Received null response" ));
                }
            }
            catch( HttpRequestException exception ) {
                mLogger.LogError( exception, String.Empty );

                dispatcher.Dispatch( new GetUsersFailureAction( exception.Message ));
            }

            dispatcher.Dispatch( new ApiCallCompleted());
        }
    }
}
