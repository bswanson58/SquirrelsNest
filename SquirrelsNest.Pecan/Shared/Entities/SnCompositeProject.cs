using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SquirrelsNest.Pecan.Shared.Entities {
    public class SnCompositeProject {
        public  SnProject                       Project { get; }
        public  IReadOnlyList<SnComponent>      Components { get; }
        public  IReadOnlyList<SnIssueType>      IssueTypes { get; }
        public  IReadOnlyList<SnWorkflowState>  WorkflowStates { get; }
        public  IReadOnlyList<SnRelease>        Releases { get; }

        public  string                          EntityId => Project.EntityId;
        public  string                          Name => Project.Name;
        public  string                          Description => Project.Description;

        [JsonConstructor]
        public SnCompositeProject( SnProject project, IReadOnlyList<SnComponent> components, IReadOnlyList<SnIssueType> issueTypes,
                                   IReadOnlyList<SnWorkflowState> workflowStates, IReadOnlyList<SnRelease> releases ) {
            Project = project;
            Components = new List<SnComponent>( components );
            IssueTypes = new List<SnIssueType>( issueTypes );
            WorkflowStates = new List<SnWorkflowState>( workflowStates );
            Releases = new List<SnRelease>( releases );
        }

        public SnCompositeProject( SnProject project ) {
            Project = project;
            Components = new List<SnComponent>();
            IssueTypes = new List<SnIssueType>();
            WorkflowStates = new List<SnWorkflowState>();
            Releases = new List<SnRelease>();
        }

        public SnCompositeProject With( IEnumerable<SnComponent> components ) =>
            new ( Project, new List<SnComponent>( components ), IssueTypes, WorkflowStates, Releases );

        public SnCompositeProject With( IEnumerable<SnIssueType> issueTypes ) =>
            new ( Project, Components, new List<SnIssueType>( issueTypes ), WorkflowStates, Releases );

        public SnCompositeProject With( IEnumerable<SnWorkflowState> states ) =>
            new( Project, Components, IssueTypes, new List<SnWorkflowState>( states ), Releases );

        private static SnCompositeProject ? mDefaultProject;

        public static SnCompositeProject Default =>
            mDefaultProject ??= new SnCompositeProject( SnProject.Default );
    }
}
