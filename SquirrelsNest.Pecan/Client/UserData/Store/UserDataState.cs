using System;
using Fluxor;

namespace SquirrelsNest.Pecan.Client.UserData.Store {
    public static class IssueListStyle {
        public  static readonly string  TitleOnly           = "titleOnly";
        public  static readonly string  TitleDescription    = "titleDescription";
        public  static readonly string  FullDetail          = "fullDetail";
    }

    [FeatureState( CreateInitialStateMethodName = "Factory" )]
    public class UserDataState {
        public  string      CurrentProjectId { get; }
        public  bool        DisplayCompletedIssues { get; }
        public  bool        DisplayCompletedIssuesLast { get; }
        public  bool        DisplayOnlyMyAssignedIssues { get; }
        public  string      IssueListDisplayStyle { get; }

        public UserDataState( string currentProjectId,
                              bool displayCompletedIssues, bool displayCompletedIssuesLast, bool displayOnlyMyAssignedIssues,
                              string issueListDisplayStyle ) {
            CurrentProjectId = currentProjectId ?? String.Empty;
            DisplayCompletedIssues = displayCompletedIssues;
            DisplayCompletedIssuesLast = displayCompletedIssuesLast;
            DisplayOnlyMyAssignedIssues = displayOnlyMyAssignedIssues;
            IssueListDisplayStyle = issueListDisplayStyle ?? IssueListStyle.FullDetail;
        }

        public static UserDataState Factory() => new ( String.Empty, true, false, false, IssueListStyle.FullDetail );
    }
}
