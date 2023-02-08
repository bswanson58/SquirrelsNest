using System.Collections.Generic;

namespace SquirrelsNest.Pecan.Server.Features.Transfer.Dto {
    internal class TransferEntities {
        public  TrProject              Project { get; set; }
        public  List<TrComponent>      Components { get; set; }
        public  List<TrIssueType>      IssueTypes { get; set; }
        public  List<TrRelease>        Releases { get; set; }
        public  List<TrWorkflowState>  WorkflowStates { get; set; }
        public  List<TrUser>           Users { get; set; }
        public  List<TrIssue>          Issues { get; set; }

        public TransferEntities() {
            Project = new TrProject();
            Components = new List<TrComponent>();
            Issues = new List<TrIssue>();
            IssueTypes = new List<TrIssueType>();
            Releases = new List<TrRelease>();
            WorkflowStates = new List<TrWorkflowState>();
            Users = new List<TrUser>();
        }

        private TransferEntities( TrProject project, 
                                  IEnumerable<TrComponent> components,
                                  IEnumerable<TrIssue> issues,
                                  IEnumerable<TrIssueType> issueTypes,
                                  IEnumerable<TrRelease> releases,
                                  IEnumerable<TrWorkflowState> states,
                                  IEnumerable<TrUser> users ) {
            Project = project;
            Components = new List<TrComponent>( components );
            Issues = new List<TrIssue>( issues );
            IssueTypes = new List<TrIssueType>( issueTypes );
            Releases = new List<TrRelease>( releases );
            WorkflowStates = new List<TrWorkflowState>( states );
            Users = new List<TrUser>( users );
        }

        public TransferEntities With( IEnumerable<TrComponent> components ) {
            return new TransferEntities( Project, components, Issues, IssueTypes, Releases, WorkflowStates, Users );
        }

        public TransferEntities With( IEnumerable<TrIssue> issues ) {
            return new TransferEntities( Project, Components, issues, IssueTypes, Releases, WorkflowStates, Users );
        }

        public TransferEntities With( IEnumerable<TrIssueType> issueTypes ) {
            return new TransferEntities( Project, Components, Issues, issueTypes, Releases, WorkflowStates, Users );
        }

        public TransferEntities With( IEnumerable<TrRelease> releases ) {
            return new TransferEntities( Project, Components, Issues, IssueTypes, releases, WorkflowStates, Users );
        }

        public TransferEntities With( IEnumerable<TrWorkflowState> states ) {
            return new TransferEntities( Project, Components, Issues, IssueTypes, Releases, states, Users );
        }

        public TransferEntities With( IEnumerable<TrUser> users ) {
            return new TransferEntities( Project, Components, Issues, IssueTypes, Releases, WorkflowStates, users );
        }
    }
}
