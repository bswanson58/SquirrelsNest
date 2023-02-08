using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using SquirrelsNest.Pecan.Client.Constants;
using SquirrelsNest.Pecan.Shared.Dto.Auth;
using SquirrelsNest.Pecan.Shared.Platform;

namespace SquirrelsNest.Pecan.Client.Auth.Support {
    public interface ITokenRefresher {
        Task<bool>      TokenRefreshRequired( int withinMinutes );
        Task<string>    RefreshToken();
    }

    public class JwtTokenRefresher : ITokenRefresher {
        private readonly AuthenticationStateProvider    mAuthenticationProvider;
        private readonly IHttpClientFactory             mClientFactory;
        private readonly ILocalStorageService           mLocalStorage;
        private readonly ILogger<JwtTokenRefresher>     mLog;

        public JwtTokenRefresher( IHttpClientFactory clientFactory, ILocalStorageService localStorage,
                                  AuthenticationStateProvider authenticationProvider, ILogger<JwtTokenRefresher> log ) {
            mAuthenticationProvider = authenticationProvider;
            mClientFactory = clientFactory;
            mLocalStorage = localStorage;
            mLog = log;
        }

        private async Task<DateTimeOffset> TokenExpirationTime() {
            var authState = await mAuthenticationProvider.GetAuthenticationStateAsync();
            var exp = authState.User.FindFirst( c => c.Type.Equals( "exp" ))?.Value;

            if(!String.IsNullOrWhiteSpace( exp )) {
                return DateTimeOffset.FromUnixTimeSeconds( Convert.ToInt64( exp ));
            }

            return DateTimeOffset.MinValue;
        }

        public async Task<bool> TokenRefreshRequired( int withinMinutes ) {
            var expTime = await TokenExpirationTime();
            var diff = expTime - DateTimeProvider.Instance.CurrentUtcTime;

            return diff.TotalMinutes <= withinMinutes;
        }

        public async Task<string> RefreshToken() {
            try {
                var token = await mLocalStorage.GetItemAsStringAsync( LocalStorageNames.AuthToken );
                var refreshToken = await mLocalStorage.GetItemAsStringAsync( LocalStorageNames.RefreshToken );
                var request = new RefreshTokenRequest{ Token = token ?? String.Empty, RefreshToken = refreshToken ?? String.Empty };

                using var httpClient = mClientFactory.CreateClient( HttpClientNames.Anonymous );
                var postResponse = await httpClient.PostAsJsonAsync( RefreshTokenRequest.Route, request );
                var response = await postResponse.Content.ReadFromJsonAsync<RefreshTokenResponse>();

                if( response?.Succeeded == true ) {
                    await mLocalStorage.SetItemAsStringAsync( LocalStorageNames.AuthToken, response.Token );
                    await mLocalStorage.SetItemAsStringAsync( LocalStorageNames.RefreshToken, response.RefreshToken );

                    return response.Token;
                }
            }
            catch( Exception ex ) {
                mLog.LogError( ex, "Attempting to refresh JWT token" );
            }

            return String.Empty;
        }
    }
}
