using Fluxor;
using SquirrelsNest.Pecan.Client.Store;

namespace SquirrelsNest.Pecan.Client.Auth.Store {
    [FeatureState( CreateInitialStateMethodName = "Factory")]
    public class AuthState : RootState {

        public AuthState( bool callInProgress, string callMessage ) :
            base( callInProgress, callMessage ) {
        }

        public static AuthState Factory() => new AuthState( false, string.Empty );

    }
}
