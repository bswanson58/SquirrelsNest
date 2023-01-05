using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.Issues.Actions {
    public class EditComponentAction {
        public  SnCompositeProject  Project { get; }
        public  SnCompositeIssue    Issue { get; }

        public EditComponentAction( SnCompositeProject project, SnCompositeIssue issue ) {
            Project = project;
            Issue = issue;
        }
    }

    public class EditIssueTypeAction {
        public  SnCompositeProject  Project { get; }
        public  SnCompositeIssue    Issue { get; }

        public EditIssueTypeAction( SnCompositeProject project, SnCompositeIssue issue ) {
            Project = project;
            Issue = issue;
        }
    }

    public class EditWorkflowStateAction {
        public  SnCompositeProject  Project { get; }
        public  SnCompositeIssue    Issue { get; }

        public EditWorkflowStateAction( SnCompositeProject project, SnCompositeIssue issue ) {
            Project = project;
            Issue = issue;
        }
    }
}
