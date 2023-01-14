using System;
using Fluxor;

namespace SquirrelsNest.Pecan.Client.UserData.Store {
    [FeatureState( CreateInitialStateMethodName = "Factory" )]
    public class UserDataState {
        public  string      CurrentProjectId { get; }
        public  bool        DisplayCompletedIssues { get; }
        public  bool        DisplayCompletedIssuesLast { get; }
        public  bool        DisplayOnlyMyAssignedIssues { get; }

        public UserDataState( string currentProjectId,
                              bool displayCompletedIssues, bool displayCompletedIssuesLast, bool displayOnlyMyAssignedIssues ) {
            CurrentProjectId = currentProjectId;
            DisplayCompletedIssues = displayCompletedIssues;
            DisplayCompletedIssuesLast = displayCompletedIssuesLast;
            DisplayOnlyMyAssignedIssues = displayOnlyMyAssignedIssues;
        }

        public static UserDataState Factory() => new ( String.Empty, true, false, false );
    }
}
