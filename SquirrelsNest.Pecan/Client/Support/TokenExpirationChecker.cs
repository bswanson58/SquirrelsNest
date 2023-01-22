using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using SquirrelsNest.Pecan.Client.Auth.Store;
using SquirrelsNest.Pecan.Shared.Platform;

namespace SquirrelsNest.Pecan.Client.Support {
    public interface ITokenExpirationChecker : IDisposable {
        void    StartChecking();
        void    StopChecking();
    }

    public class TokenExpirationChecker : ITokenExpirationChecker {
        private const int                   CheckTimeInMinutes = 1;

        private readonly AuthenticationStateProvider    mAuthenticationProvider;
        private readonly AuthFacade                     mAuthFacade;
        private Timer ?                                 mTimer;
        private bool                                    mDidIt;

        public TokenExpirationChecker( AuthFacade authFacade, AuthenticationStateProvider authenticationProvider ) {
            mAuthFacade = authFacade;
            mAuthenticationProvider = authenticationProvider;

            mTimer = null;
            mDidIt = false;
        }

        public void StartChecking() {
            StopChecking();

            mDidIt = false;

            mTimer = new Timer( OnTimer, null, 
                TimeSpan.FromMinutes( CheckTimeInMinutes ), 
                TimeSpan.FromMinutes( CheckTimeInMinutes ));
        }

        private async void OnTimer( object ? state ) {
            if( await TokenRefreshImmanent( 1 )) {
                if(!mDidIt ) {
                    mAuthFacade.LogoutUser();

                    mDidIt = true;
                }
            }
            else {
                mDidIt = false;
            }
        }

        private async Task<bool> TokenRefreshImmanent( int withinMinutes ) {
            var expTime = await TokenExpirationTime();
            var diff = expTime - DateTimeProvider.Instance.CurrentUtcTime;

            return diff.TotalMinutes <= withinMinutes;
        }

        private async Task<DateTimeOffset> TokenExpirationTime() {
            var authState = await mAuthenticationProvider.GetAuthenticationStateAsync();
            var exp = authState.User.FindFirst( c => c.Type.Equals( "exp" ))?.Value;

            if(!String.IsNullOrWhiteSpace( exp )) {
                return DateTimeOffset.FromUnixTimeSeconds( Convert.ToInt64( exp ));
            }

            return DateTimeOffset.MinValue;
        }

        public void StopChecking() {
            mTimer?.Dispose();
            mTimer = null;
        }

        public void Dispose() {
            StopChecking();
        }
    }
}
