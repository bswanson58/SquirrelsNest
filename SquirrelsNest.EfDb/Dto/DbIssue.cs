using System.Diagnostics;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Platform;

namespace SquirrelsNest.EfDb.Dto {
    [DebuggerDisplay("{" + nameof( Title ) + "}")]
    internal class DbIssue : DbBase {
        public  string      Title { get; set; }
        public  string      Description { get; set; }
        public  string      ProjectId { get; set; }
        public  uint        IssueNumber {  get; set; }
        public  DateOnly    EntryDate { get; set; }
        public  string      EnteredById { get; set; }
        public  string      IssueTypeId { get; set; }
        public  string      ComponentId { get; set; }
        public  string      ReleaseId { get; set; }
        public  string      WorkflowStateId { get; set; }
        public  string      AssignedToId { get; set; }

        public DbIssue() {
            Title = String.Empty;
            Description = String.Empty;
            ProjectId = Common.Values.EntityId.Default;
            IssueNumber = 0;
            EntryDate = DateTimeProvider.Instance.CurrentDate;
            EnteredById = Common.Values.EntityId.Default;
            IssueTypeId = Common.Values.EntityId.Default;
            ComponentId = Common.Values.EntityId.Default;
            ReleaseId = Common.Values.EntityId.Default;
            WorkflowStateId = Common.Values.EntityId.Default;
            AssignedToId = Common.Values.EntityId.Default;
        }

        public static DbIssue From( SnIssue issue ) {
            if( issue == null ) throw new ApplicationException( "source issue cannot be null" );

            return new DbIssue {
                EntityId = issue.EntityId,
                Id = String.IsNullOrWhiteSpace( issue.DbId ) ? DbIdDefault() : DbIdCreate( issue.DbId ),
                Title = issue.Title,
                Description = issue.Description,
                ProjectId = issue.ProjectId,
                IssueNumber = issue.IssueNumber,
                EntryDate = issue.EntryDate,
                EnteredById = issue.EnteredById,
                IssueTypeId = issue.IssueTypeId,
                ComponentId = issue.ComponentId,
                ReleaseId = issue.ReleaseId,
                WorkflowStateId = issue.WorkflowStateId,
                AssignedToId = issue.AssignedToId
            };
        }

        public SnIssue ToEntity() {
            return new SnIssue( EntityId, Id.ToString(), Title, Description, ProjectId, IssueNumber, EntryDate,
                                Common.Values.EntityId.CreateIdOrThrow( EnteredById ),
                                Common.Values.EntityId.CreateIdOrThrow( IssueTypeId ),
                                Common.Values.EntityId.CreateIdOrThrow( ComponentId ), 
                                Common.Values.EntityId.CreateIdOrThrow( ReleaseId ), 
                                Common.Values.EntityId.CreateIdOrThrow( WorkflowStateId ),
                                Common.Values.EntityId.CreateIdOrThrow( AssignedToId ));
        }
    }
}
