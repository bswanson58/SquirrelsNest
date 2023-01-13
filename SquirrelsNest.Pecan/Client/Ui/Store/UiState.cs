using Fluxor;

namespace SquirrelsNest.Pecan.Client.Ui.Store {
    [FeatureState( CreateInitialStateMethodName = "Factory" )]
    public class UiState {
        public  bool    DisplayCompletedIssues { get; }
        public  bool    DisplayCompletedIssuesLast { get; }
        public  bool    DisplayOnlyMyAssignedIssues { get; }

        public UiState( bool displayCompletedIssues, bool displayCompletedIssuesLast,
                        bool displayOnlyMyAssignedIssues ) {
            DisplayCompletedIssues = displayCompletedIssues;
            DisplayCompletedIssuesLast = displayCompletedIssuesLast;
            DisplayOnlyMyAssignedIssues = displayOnlyMyAssignedIssues;
        }

        public static UiState Factory() => new ( true, true, false );
    }
}
