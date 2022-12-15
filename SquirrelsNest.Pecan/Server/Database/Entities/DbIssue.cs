using SquirrelsNest.Pecan.Shared.Entities;
using System;
using SquirrelsNest.Pecan.Shared.Platform;

namespace SquirrelsNest.Pecan.Server.Database.Entities {
    public class DbIssue : DbEntityBase {
        public  string      Title { get; set; }
        public  string      Description { get; set; }
        public  string      ProjectId { get; set; }
        public  uint        IssueNumber {  get; set; }
        public  DateOnly    EntryDate { get; set; }
        public  string      EnteredById { get; set; }
        public  string      IssueTypeId {  get; set; }
        public  string      ComponentId { get; set; }
        public  string      ReleaseId { get; set; }
        public  string      WorkflowStateId { get; set; }
        public  string      AssignedToId { get; set; }

        public DbIssue() {
            Title = String.Empty;
            Description = String.Empty;
            ProjectId = String.Empty;
            IssueNumber = 0;
            EntryDate = DateTimeProvider.Instance.CurrentDate;
            EnteredById = String.Empty;
            IssueTypeId = String.Empty;
            ComponentId = String.Empty;
            ReleaseId = String.Empty;
            WorkflowStateId = String.Empty;
            AssignedToId = String.Empty;
        }

        public DbIssue( SnIssue issue ) :
            base( issue.EntityId ) {
            Title = issue.Title;
            Description = issue.Description;
            ProjectId = issue.ProjectId;
            IssueNumber = issue.IssueNumber;
            EntryDate = issue.EntryDate;
            EnteredById = issue.EnteredById;
            IssueTypeId = issue.IssueTypeId;
            ComponentId = issue.ComponentId;
            ReleaseId = issue.ReleaseId;
            WorkflowStateId = issue.WorkflowStateId;
            AssignedToId = issue.AssignedToId;
        }

        public static DbIssue From( SnIssue issue ) => new DbIssue( issue );

        public SnIssue ToEntity() => new SnIssue( EntityId, Title, Description, ProjectId, IssueNumber, EntryDate, 
                                                  EnteredById, IssueTypeId, ComponentId, ReleaseId, WorkflowStateId, AssignedToId );
    }
}
