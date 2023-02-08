namespace SquirrelsNest.Pecan.Client.Issues.Actions {
    public class SetIssueListPageAction {
        public  uint    PageNumber { get; }

        public SetIssueListPageAction( uint toPage ) {
            PageNumber = toPage;
        }
    }
}
