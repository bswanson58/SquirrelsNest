﻿using System;
using Fluxor;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;
using SquirrelsNest.Pecan.Client.Auth.Actions;
using SquirrelsNest.Pecan.Client.Support;
using SquirrelsNest.Pecan.Shared.Dto.Auth;
using SquirrelsNest.Pecan.Client.Ui.Actions;

namespace SquirrelsNest.Pecan.Client.Auth.Effects {
    // ReSharper disable once UnusedType.Global
    public class CreateUserSubmitEffect : Effect<CreateUserSubmitAction> {
        private readonly    ILogger<CreateUserSubmitEffect> mLogger;
        private readonly    IAnonymousHttpHandler           mHttpClient;

        public CreateUserSubmitEffect( IAnonymousHttpHandler httpClient, ILogger<CreateUserSubmitEffect> logger ) {
            mHttpClient = httpClient;
            mLogger = logger;
        }

        public override async Task HandleAsync( CreateUserSubmitAction action, IDispatcher dispatcher ) {
            dispatcher.Dispatch( new ApiCallStarted( "Registering New User" ));

            try {
                var response = await mHttpClient.Post<CreateUserResponse>( CreateUserInput.Route, action.UserInput );

                if( response != null ) {
                    if( response.Succeeded ) {
                        dispatcher.Dispatch( new CreateUserSuccessAction());
                    }
                    else {
                        dispatcher.Dispatch( new CreateUserFailureAction( response.Message ));
                    }
                }
                else {
                    dispatcher.Dispatch( new CreateUserFailureAction( response?.Message ?? "Received null response" ));
                }
            }
            catch ( HttpRequestException exception ) {
                mLogger.LogError( exception, String.Empty );

                dispatcher.Dispatch( new CreateUserFailureAction( exception.Message ));
            }

            dispatcher.Dispatch( new ApiCallCompleted());
        }
    }
}
