using System.Text.Json.Serialization;
using SquirrelsNest.Core.CompositeBuilders;

namespace SquirrelsNest.Core.Transfer.Dto {
    internal class TransferEntities {
        [JsonIgnore]
        public  CompositeProject                CompositeProject { get; }
        public  TrProject                       Project { get; }
        public  IReadOnlyList<TrComponent>      Components { get; }
        public  IReadOnlyList<TrIssueType>      IssueTypes { get; }
        public  IReadOnlyList<TrRelease>        Releases { get; }
        public  IReadOnlyList<TrWorkflowState>  States { get; }
        public  IReadOnlyList<TrIssue>          Issues { get; }

        public TransferEntities( CompositeProject forProject ) {
            CompositeProject = forProject;
            Project = TrProject.From( forProject.Project );

            Components = new List<TrComponent>();
            Issues = new List<TrIssue>();
            IssueTypes = new List<TrIssueType>();
            Releases = new List<TrRelease>();
            States = new List<TrWorkflowState>();
        }

        private TransferEntities( CompositeProject project, 
                                  IEnumerable<TrComponent> components,
                                  IEnumerable<TrIssue> issues,
                                  IEnumerable<TrIssueType> issueTypes,
                                  IEnumerable<TrRelease> releases,
                                  IEnumerable<TrWorkflowState> states ) {
            CompositeProject = project;
            Project = TrProject.From( CompositeProject.Project );
            Components = new List<TrComponent>( components );
            Issues = new List<TrIssue>( issues );
            IssueTypes = new List<TrIssueType>( issueTypes );
            Releases = new List<TrRelease>( releases );
            States = new List<TrWorkflowState>( states );
        }

        public TransferEntities With( IEnumerable<TrComponent> components ) {
            return new TransferEntities( CompositeProject, components, Issues, IssueTypes, Releases, States );
        }

        public TransferEntities With( IEnumerable<TrIssue> issues ) {
            return new TransferEntities( CompositeProject, Components, issues, IssueTypes, Releases, States );
        }

        public TransferEntities With( IEnumerable<TrIssueType> issueTypes ) {
            return new TransferEntities( CompositeProject, Components, Issues, issueTypes, Releases, States );
        }

        public TransferEntities With( IEnumerable<TrRelease> releases ) {
            return new TransferEntities( CompositeProject, Components, Issues, IssueTypes, releases, States );
        }

        public TransferEntities With( IEnumerable<TrWorkflowState> states ) {
            return new TransferEntities( CompositeProject, Components, Issues, IssueTypes, Releases, states );
        }
    }
}
