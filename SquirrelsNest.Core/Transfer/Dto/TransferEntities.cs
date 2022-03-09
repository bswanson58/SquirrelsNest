using System.Text.Json.Serialization;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Core.CompositeBuilders;

namespace SquirrelsNest.Core.Transfer.Dto {
    internal class TransferEntities {
        [JsonIgnore]
        public  CompositeProject                CompositeProject { get; }
        public  TrProject                       Project { get; set; }
        public  List<TrComponent>      Components { get; set; }
        public  List<TrIssueType>      IssueTypes { get; set; }
        public  List<TrRelease>        Releases { get; set; }
        public  List<TrWorkflowState>  States { get; set; }
        public  List<TrUser>           Users { get; set; }
        public  List<TrIssue>          Issues { get; set; }

        public TransferEntities() {
            CompositeProject = new CompositeProject( SnProject.Default, Array.Empty<SnIssueType>(), Array.Empty<SnComponent>(), 
                                   Array.Empty<SnWorkflowState>(), Array.Empty<SnRelease>(), Array.Empty<SnUser>());
            Project = new TrProject();
            Components = new List<TrComponent>();
            Issues = new List<TrIssue>();
            IssueTypes = new List<TrIssueType>();
            Releases = new List<TrRelease>();
            States = new List<TrWorkflowState>();
            Users = new List<TrUser>();
        }

        public TransferEntities( CompositeProject forProject ) {
            CompositeProject = forProject;
            Project = TrProject.From( forProject.Project );

            Components = new List<TrComponent>();
            Issues = new List<TrIssue>();
            IssueTypes = new List<TrIssueType>();
            Releases = new List<TrRelease>();
            States = new List<TrWorkflowState>();
            Users = new List<TrUser>();
        }

        private TransferEntities( CompositeProject compositeProject, TrProject project, 
                                  IEnumerable<TrComponent> components,
                                  IEnumerable<TrIssue> issues,
                                  IEnumerable<TrIssueType> issueTypes,
                                  IEnumerable<TrRelease> releases,
                                  IEnumerable<TrWorkflowState> states,
                                  IEnumerable<TrUser> users ) {
            CompositeProject = compositeProject;
            Project = project;
            Components = new List<TrComponent>( components );
            Issues = new List<TrIssue>( issues );
            IssueTypes = new List<TrIssueType>( issueTypes );
            Releases = new List<TrRelease>( releases );
            States = new List<TrWorkflowState>( states );
            Users = new List<TrUser>( users );
        }

        public TransferEntities With( IEnumerable<TrComponent> components ) {
            return new TransferEntities( CompositeProject, Project, components, Issues, IssueTypes, Releases, States, Users );
        }

        public TransferEntities With( IEnumerable<TrIssue> issues ) {
            return new TransferEntities( CompositeProject, Project, Components, issues, IssueTypes, Releases, States, Users );
        }

        public TransferEntities With( IEnumerable<TrIssueType> issueTypes ) {
            return new TransferEntities( CompositeProject, Project, Components, Issues, issueTypes, Releases, States, Users );
        }

        public TransferEntities With( IEnumerable<TrRelease> releases ) {
            return new TransferEntities( CompositeProject, Project, Components, Issues, IssueTypes, releases, States, Users );
        }

        public TransferEntities With( IEnumerable<TrWorkflowState> states ) {
            return new TransferEntities( CompositeProject, Project, Components, Issues, IssueTypes, Releases, states, Users );
        }

        public TransferEntities With( IEnumerable<TrUser> users ) {
            return new TransferEntities( CompositeProject, Project, Components, Issues, IssueTypes, Releases, States, users );
        }
    }
}
