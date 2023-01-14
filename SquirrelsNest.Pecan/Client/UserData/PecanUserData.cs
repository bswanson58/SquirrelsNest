using System;
using System.Text.Json.Serialization;
using SquirrelsNest.Pecan.Client.UserData.Store;

namespace SquirrelsNest.Pecan.Client.UserData {
    public class PecanUserData {
        public  string  CurrentProjectId { get; }
        public  bool    DisplayCompletedIssues { get; }
        public  bool    DisplayCompletedIssuesLast { get; }
        public  bool    DisplayOnlyMyAssignedIssues { get; }
        public  string  IssueListDisplayStyle { get; }

        [JsonConstructor]
        public PecanUserData( string currentProjectId, bool displayCompletedIssues,
                              bool displayCompletedIssuesLast, bool displayOnlyMyAssignedIssues,
                              string issueListDisplayStyle ) {
            CurrentProjectId = currentProjectId ?? String.Empty;
            DisplayCompletedIssues = displayCompletedIssues;
            DisplayCompletedIssuesLast = displayCompletedIssuesLast;
            DisplayOnlyMyAssignedIssues = displayOnlyMyAssignedIssues;
            IssueListDisplayStyle = issueListDisplayStyle ?? IssueListStyle.FullDetail;
        }

        public PecanUserData() {
            CurrentProjectId = string.Empty;

            DisplayCompletedIssues = true;
            DisplayCompletedIssuesLast = false;
            DisplayOnlyMyAssignedIssues = false;

            IssueListDisplayStyle = IssueListStyle.FullDetail;
        }
    }
}
