using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.Core.CompositeBuilders {
    public class CompositeIssue {
        public  SnProject           Project { get; }
        public  SnIssue             Issue { get; }
        public  SnIssueType         IssueType { get; }
        public  SnWorkflowState     State { get; }

        public CompositeIssue( SnProject project, SnIssue issue, SnIssueType issueType, SnWorkflowState state ) {
            Project = project;
            Issue = issue;
            IssueType = issueType;
            State = state;
        }
    }
}
