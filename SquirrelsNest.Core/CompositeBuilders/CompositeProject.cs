using System.Diagnostics;
using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.Core.CompositeBuilders {
    [DebuggerDisplay("{" + nameof( DebugName ) + "}")]
    public class CompositeProject {
        public  SnProject                       Project { get; }
        public  IReadOnlyList<SnIssueType>      IssueTypes { get; }
        public  IReadOnlyList<SnComponent>      Components { get; }
        public  IReadOnlyList<SnWorkflowState>  WorkflowStates { get; }
        public  IReadOnlyList<SnRelease>        Releases { get; }
        public  IReadOnlyList<SnUser>           Users { get; }

        public  string                          DebugName => $"Project: {Project.Name}";

        public CompositeProject( SnProject project, IEnumerable<SnIssueType> issueTypes, IEnumerable<SnComponent> components,
                                 IEnumerable<SnWorkflowState> states, IEnumerable<SnRelease> releases, IEnumerable<SnUser> users ) {
            Project = project;
            IssueTypes = new List<SnIssueType>( issueTypes );
            Components = new List<SnComponent>( components );
            WorkflowStates = new List<SnWorkflowState>( states );
            Releases = new List<SnRelease>( releases );
            Users = new List<SnUser>( users );
        }
    }
}
