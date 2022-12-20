using System;
using Fluxor;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SquirrelsNest.Pecan.Client.Auth.Actions;
using SquirrelsNest.Pecan.Shared.Dto.Auth;

namespace SquirrelsNest.Pecan.Client.Auth.Effects {
    // ReSharper disable once UnusedType.Global
    public class CreateUserSubmitEffect : Effect<CreateUserSubmitAction> {
        private readonly    ILogger<CreateUserSubmitEffect> mLogger;
        private readonly    HttpClient                      mHttpClient;

        public CreateUserSubmitEffect( HttpClient httpClient, ILogger<CreateUserSubmitEffect> logger ) {
            mHttpClient = httpClient;
            mLogger = logger;
        }

        public override async Task HandleAsync( CreateUserSubmitAction action, IDispatcher dispatcher ) {
            try {
                var postResponse = await mHttpClient.PostAsJsonAsync( CreateUserInput.Route, action.UserInput );
                var response = await postResponse.Content.ReadFromJsonAsync<CreateUserResponse>();

                if( response != null ) {
                    if( response.Succeeded ) {
                        dispatcher.Dispatch( new CreateUserSuccessAction());
                    }
                    else {
                        dispatcher.Dispatch( new CreateUserFailureAction( response.Message ));
                    }
                }
                else {
                    dispatcher.Dispatch( new CreateUserFailureAction( "Received null response" ));
                }
            }
            catch ( HttpRequestException exception ) {
                mLogger.LogError( exception, String.Empty );

                dispatcher.Dispatch( new CreateUserFailureAction( exception.Message ));
            }
        }
    }
}
