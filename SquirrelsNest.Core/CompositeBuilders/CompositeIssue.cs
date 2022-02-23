using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.Core.CompositeBuilders {
    public class CompositeIssue {
        public  SnProject           Project { get; }
        public  SnIssue             Issue { get; }
        public  SnIssueType         IssueType { get; }
        public  SnComponent         Component { get; }
        public  SnWorkflowState     State { get; }

        public CompositeIssue( SnProject project, SnIssue issue, SnIssueType issueType, SnComponent component, SnWorkflowState state ) {
            Project = project;
            Issue = issue;
            IssueType = issueType;
            Component = component;
            State = state;
        }
    }
}
