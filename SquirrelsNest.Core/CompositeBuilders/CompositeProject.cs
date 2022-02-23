using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.Core.CompositeBuilders {
    public class CompositeProject {
        public  SnProject                       Project { get; }
        public  IReadOnlyList<SnIssueType>      IssueTypes { get; }
        public  IReadOnlyList<SnComponent>      Components { get; }
        public  IReadOnlyList<SnWorkflowState>  WorkflowStates { get; }
        public  IReadOnlyList<SnUser>           Users { get; }

        public CompositeProject( SnProject project, IEnumerable<SnIssueType> issueTypes, IEnumerable<SnComponent> components,
                                 IEnumerable<SnWorkflowState> states, IEnumerable<SnUser> users ) {
            Project = project;
            IssueTypes = new List<SnIssueType>( issueTypes );
            Components = new List<SnComponent>( components );
            WorkflowStates = new List<SnWorkflowState>( states );
            Users = new List<SnUser>( users );
        }
    }
}
