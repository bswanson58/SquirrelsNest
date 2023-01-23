using System;
using System.Threading;
using SquirrelsNest.Pecan.Client.Auth.Store;
using SquirrelsNest.Pecan.Client.Auth.Support;

namespace SquirrelsNest.Pecan.Client.Support {
    public interface ITokenExpirationChecker : IDisposable {
        void    StartChecking();
        void    StopChecking();
    }

    public class TokenExpirationChecker : ITokenExpirationChecker {
        private const int                   CheckTimeInMinutes = 1;

        private readonly IAuthInformation   mAuthInformation;
        private readonly AuthFacade         mAuthFacade;
        private Timer ?                     mTimer;
        private bool                        mDidIt;

        public TokenExpirationChecker( AuthFacade authFacade, IAuthInformation authInformation ) {
            mAuthFacade = authFacade;
            mAuthInformation = authInformation;

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

        private void OnTimer( object ? state ) {
            if( TokenRefreshImmanent( TimeSpan.FromMinutes( CheckTimeInMinutes ))) {
                if(!mDidIt ) {
                    mAuthFacade.LogoutUser();

                    mDidIt = true;
                }
            }
            else {
                mDidIt = false;
            }
        }

        private bool TokenRefreshImmanent( TimeSpan timeOffset ) =>
            mAuthInformation.TimeOffsetToTokenExpiration < timeOffset;

        public void StopChecking() {
            mTimer?.Dispose();
            mTimer = null;
        }

        public void Dispose() {
            StopChecking();
        }
    }
}
