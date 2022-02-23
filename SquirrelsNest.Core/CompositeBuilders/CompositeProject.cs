using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.Core.CompositeBuilders {
    public class CompositeProject {
        public  SnProject                       Project { get; }
        public  IReadOnlyList<SnIssueType>      IssueTypes { get; }
        public  IReadOnlyList<SnWorkflowState>  WorkflowStates { get; }

        public CompositeProject( SnProject project, IEnumerable<SnIssueType> issueTypes, IEnumerable<SnWorkflowState> states ) {
            Project = project;
            IssueTypes = new List<SnIssueType>( issueTypes );
            WorkflowStates = new List<SnWorkflowState>( states );
        }
    }
}
