using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.Core.CompositeBuilders {
    public class CompositeIssue {
        public  SnProject           Project { get; }
        public  SnIssue             Issue { get; }
        public  SnIssueType         IssueType { get; }
        public  SnComponent         Component { get; }
        public  SnWorkflowState     State { get; }
        public  SnUser              EnteredBy { get; }
        public  SnUser              AssignedTo { get; }

        public CompositeIssue( SnProject project, SnIssue issue, SnIssueType issueType, SnUser enteredBy, SnComponent component, SnWorkflowState state,
                               SnUser assignedTo ) {
            Project = project;
            Issue = issue;
            IssueType = issueType;
            EnteredBy = enteredBy;
            Component = component;
            State = state;
            AssignedTo = assignedTo;
        }
    }
}
