using System;
using Fluxor;
using SquirrelsNest.Pecan.Client.Store;

namespace SquirrelsNest.Pecan.Client.UserData.Store {
    [FeatureState( CreateInitialStateMethodName = "Factory" )]
    public class UserDataState : RootState {
        public  string      CurrentProjectId { get; }

        public UserDataState( bool callInProgress, string callMessage, string currentProjectId ) :
            base( callInProgress, callMessage ) {
            CurrentProjectId = currentProjectId;
        }

        public static UserDataState Factory() => new ( false, String.Empty, String.Empty );
    }
}
