using System;
using Fluxor;
using SquirrelsNest.Pecan.Client.Store;

namespace SquirrelsNest.Pecan.Client.UserData.Store {
    [FeatureState( CreateInitialStateMethodName = "Factory" )]
    public class UserDataState : RootState {
        public  string      CurrentProjectId { get; }
        public  bool        DisplayCompletedIssues { get; }
        public  bool        DisplayCompletedIssuesLast { get; }
        public  bool        DisplayOnlyMyAssignedIssues { get; }

        public UserDataState( bool callInProgress, string callMessage, string currentProjectId,
            bool displayCompletedIssues, bool displayCompletedIssuesLast, bool displayOnlyMyAssignedIssues ) :
            base( callInProgress, callMessage ) {
            CurrentProjectId = currentProjectId;
            DisplayCompletedIssues = displayCompletedIssues;
            DisplayCompletedIssuesLast = displayCompletedIssuesLast;
            DisplayOnlyMyAssignedIssues = displayOnlyMyAssignedIssues;
        }

        public static UserDataState Factory() => new ( false, String.Empty, String.Empty, true, false, false );
    }
}
