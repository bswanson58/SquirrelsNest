using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace SquirrelsNest.Pecan.Shared.Entities {
    public class SnCompositeProject {
        public  SnProject                       Project { get; }
        public  IReadOnlyList<SnComponent>      Components { get; }
        public  IReadOnlyList<SnIssueType>      IssueTypes { get; }
        public  IReadOnlyList<SnWorkflowState>  WorkflowStates { get; }
        public  IReadOnlyList<SnRelease>        Releases { get; }
        public  IReadOnlyList<SnUser>           Users { get; }

        public  string                          EntityId => Project.EntityId;
        public  string                          Name => Project.Name;
        public  string                          Description => Project.Description;

        [JsonConstructor]
        public SnCompositeProject( SnProject project, IReadOnlyList<SnComponent> components, IReadOnlyList<SnIssueType> issueTypes,
                                   IReadOnlyList<SnWorkflowState> workflowStates, IReadOnlyList<SnRelease> releases,
                                   IReadOnlyList<SnUser> users ) {
            Project = project;
            Components = new List<SnComponent>( components );
            IssueTypes = new List<SnIssueType>( issueTypes );
            WorkflowStates = new List<SnWorkflowState>( workflowStates );
            Releases = new List<SnRelease>( releases );
            Users = new List<SnUser>( users );
        }

        public SnCompositeProject( SnProject project ) {
            Project = project;
            Components = new List<SnComponent>();
            IssueTypes = new List<SnIssueType>();
            WorkflowStates = new List<SnWorkflowState>();
            Releases = new List<SnRelease>();
            Users = new List<SnUser>();
        }

        public SnCompositeProject With( IEnumerable<SnComponent> components ) =>
            new ( Project, new List<SnComponent>( components ), IssueTypes, WorkflowStates, Releases, Users );

        public SnCompositeProject With( IEnumerable<SnIssueType> issueTypes ) =>
            new ( Project, Components, new List<SnIssueType>( issueTypes ), WorkflowStates, Releases, Users );

        public SnCompositeProject With( IEnumerable<SnWorkflowState> states ) =>
            new( Project, Components, IssueTypes, new List<SnWorkflowState>( states ), Releases, Users );

        private static SnCompositeProject ? mDefaultProject;

        public static SnCompositeProject Default =>
            mDefaultProject ??= new SnCompositeProject( SnProject.Default );

        public bool IsViableProject() => Components.Any() && IssueTypes.Any() && WorkflowStates.Any();
    }
}
