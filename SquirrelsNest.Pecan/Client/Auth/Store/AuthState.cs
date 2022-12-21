using System;
using Fluxor;
using SquirrelsNest.Pecan.Client.Store;
using SquirrelsNest.Pecan.Shared.Platform;

namespace SquirrelsNest.Pecan.Client.Auth.Store {
    [FeatureState( CreateInitialStateMethodName = "Factory")]
    public class AuthState : RootState {
        public  string      UserToken { get; }
        public  DateTime    TokenExpiration { get; }

        public AuthState( bool callInProgress, string callMessage, string userToken, DateTime tokenExpiration ) :
            base( callInProgress, callMessage ) {
            UserToken = userToken;
            TokenExpiration = tokenExpiration;
        }

        public static AuthState Factory() => 
            new( false, string.Empty, String.Empty, DateTimeProvider.Instance.CurrentDateTime );
    }
}
