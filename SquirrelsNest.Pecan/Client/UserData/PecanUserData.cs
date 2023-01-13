using System.Text.Json.Serialization;

namespace SquirrelsNest.Pecan.Client.UserData {
    public class PecanUserData {
        public  string  CurrentProjectId { get; }
        public  bool    DisplayCompletedIssues { get; }
        public  bool    DisplayCompletedIssuesLast { get; }
        public  bool    DisplayOnlyMyAssignedIssues { get; }

        [JsonConstructor]
        public PecanUserData( string currentProjectId, bool displayCompletedIssues,
                              bool displayCompletedIssuesLast, bool displayOnlyMyAssignedIssues ) {
            CurrentProjectId = currentProjectId;
            DisplayCompletedIssues = displayCompletedIssues;
            DisplayCompletedIssuesLast = displayCompletedIssuesLast;
            DisplayOnlyMyAssignedIssues = displayOnlyMyAssignedIssues;
        }

        public PecanUserData() {
            CurrentProjectId = string.Empty;

            DisplayCompletedIssues = true;
            DisplayCompletedIssuesLast = false;
            DisplayOnlyMyAssignedIssues = false;
        }
    }
}
