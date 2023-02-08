using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.Issues.Actions {
    public class EditAssignedUserAction {
        public  SnCompositeProject  Project { get; }
        public  SnCompositeIssue    Issue { get; }

        public EditAssignedUserAction( SnCompositeProject project, SnCompositeIssue issue ) {
            Project = project;
            Issue = issue;
        }
    }
}
