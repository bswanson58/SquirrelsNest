﻿using System;
using Fluxor;
using Microsoft.Extensions.Logging;
using SquirrelsNest.Pecan.Client.Auth.Actions;
using SquirrelsNest.Pecan.Shared.Dto.Auth;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SquirrelsNest.Pecan.Client.Auth.Effects {
    // ReSharper disable once UnusedType.Global
    public class LoginUserSubmitEffect : Effect<LoginUserSubmitAction> {
        private readonly ILogger<LoginUserSubmitEffect> mLogger;
        private readonly HttpClient                     mHttpClient;

        public LoginUserSubmitEffect( HttpClient httpClient, ILogger<LoginUserSubmitEffect> logger ) {
            mHttpClient = httpClient;
            mLogger = logger;
        }

        public override async Task HandleAsync( LoginUserSubmitAction action, IDispatcher dispatcher ) {
            try {
                var postResponse = await mHttpClient.PostAsJsonAsync( LoginUserInput.Route, action.UserInput );
                var response = await postResponse.Content.ReadFromJsonAsync<LoginUserResponse>();

                if( response != null ) {
                    if( response.Succeeded ) {
                        dispatcher.Dispatch( new LoginUserSuccessAction( response ));
                    }
                    else {
                        dispatcher.Dispatch( new LoginUserFailureAction( response.Message ));
                    }
                }
                else {
                    dispatcher.Dispatch( new LoginUserFailureAction( "Received null response" ));
                }
            }
            catch( HttpRequestException exception ) {
                mLogger.LogError( exception, String.Empty );

                dispatcher.Dispatch( new LoginUserFailureAction( exception.Message ));
            }
        }
    }
}