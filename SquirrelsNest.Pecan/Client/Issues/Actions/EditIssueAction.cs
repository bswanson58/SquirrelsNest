using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.Issues.Actions {
    public class EditIssueAction {
        public  SnCompositeProject  Project { get; }
        public  SnCompositeIssue    Issue { get; }

        public EditIssueAction( SnCompositeProject project, SnCompositeIssue issue ) {
            Project = project;
            Issue = issue;
        }
    }
}
