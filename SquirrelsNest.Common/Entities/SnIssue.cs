using System.Diagnostics;
using SquirrelsNest.Common.Platform;
using SquirrelsNest.Common.Values;

namespace SquirrelsNest.Common.Entities {
    [DebuggerDisplay("Issue = {" + nameof( Title ) + "}")]
    public class SnIssue : EntityBase {
        public  string      Title { get; }
        public  string      Description { get; }
        public  EntityId    ProjectId { get; }
        public  int         IssueNumber {  get; }
        public  DateOnly    EntryDate { get; }
        public  EntityId    IssueTypeId {  get; }
        public  EntityId    ReleaseId { get; }
        public  EntityId    WorkflowStateId { get; }

        // the serializable constructor
        public SnIssue( string entityId, string dbId, string title, string description, string projectId, int issueNumber, DateOnly entryDate, 
                        EntityId issueTypeId, EntityId releaseId, EntityId workflowStateId )
            : base( entityId, dbId ) {
            ProjectId = EntityId.CreateIdOrThrow( projectId );
            IssueTypeId = EntityId.CreateIdOrThrow( issueTypeId );
            Title = title;
            Description = description;
            IssueNumber = issueNumber;
            EntryDate = entryDate;
            ReleaseId = releaseId;
            WorkflowStateId = workflowStateId;
        }

        public SnIssue( string title, int issueNumber, EntityId projectId ) :
            base( String.Empty ) {
            if( String.IsNullOrWhiteSpace( title )) throw new ApplicationException( "Issue titles cannot be empty" );

            Title = title;
            IssueNumber = issueNumber;
            ProjectId = EntityId.CreateIdOrThrow( projectId );

            Description = String.Empty;
            EntryDate = DateTimeProvider.Instance.CurrentDate;
            IssueTypeId = EntityId.Default;
            ReleaseId = EntityId.Default;
            WorkflowStateId = EntityId.Default;
        }

        public SnIssue With( string ? title = null, string ? description = null ) {
            return new SnIssue( 
                EntityId, DbId,
                title ?? Title,
                description ?? Description,
                ProjectId,
                IssueNumber,
                EntryDate,
                IssueTypeId,
                ReleaseId,
                WorkflowStateId );
        }

        public SnIssue With( SnRelease release ) {
            if( release == null ) throw new ApplicationException( "Release for issue cannot be null" );

            return new SnIssue( EntityId, DbId, Title, Description, ProjectId, IssueNumber, EntryDate, IssueTypeId, release.EntityId, WorkflowStateId );
        }

        public SnIssue With( SnWorkflowState state ) {
            if( state == null ) throw new ArgumentNullException( nameof( state ), "Workflow state for issue cannot be null" );

            return new SnIssue( EntityId, DbId, Title, Description, ProjectId, IssueNumber, EntryDate, IssueTypeId, ReleaseId, state.EntityId );
        }

        public SnIssue With( SnIssueType type ) {
            if( type == null ) throw new ArgumentNullException( nameof( type ), "IssueType for issue cannot be null" );

            return new SnIssue( EntityId, DbId, Title, Description, ProjectId, IssueNumber, EntryDate, type.EntityId, ReleaseId, WorkflowStateId );
        }
    }
}
