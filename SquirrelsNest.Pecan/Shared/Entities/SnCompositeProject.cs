using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SquirrelsNest.Pecan.Shared.Entities {
    public class SnCompositeProject {
        public  SnProject               Project { get; }
        public  List<SnComponent>       Components { get; }
        public  List<SnIssueType>       IssueTypes { get; }
        public  List<SnWorkflowState>   WorkflowStates { get; }
        public  List<SnRelease>         Releases { get; }

        public  string                  EntityId => Project.EntityId;
        public  string                  Name => Project.Name;
        public  string                  Description => Project.Description;

        [JsonConstructor]
        public SnCompositeProject( SnProject project, List<SnComponent> components, List<SnIssueType> issueTypes,
                                   List<SnWorkflowState> workflowStates, List<SnRelease> releases ) {
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
    }
}
