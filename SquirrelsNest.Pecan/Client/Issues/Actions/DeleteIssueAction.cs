using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.Issues.Actions {
    public class DeleteIssueAction {
        public  SnCompositeIssue    Issue { get; }

        public DeleteIssueAction( SnCompositeIssue issue ) {
            Issue = issue;
        }
    }
}
