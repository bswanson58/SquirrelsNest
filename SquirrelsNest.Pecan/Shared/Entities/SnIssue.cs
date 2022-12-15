using System;
using System.Diagnostics;
using SquirrelsNest.Pecan.Shared.Platform;

namespace SquirrelsNest.Pecan.Shared.Entities {
    [DebuggerDisplay("Issue: {" + nameof( Title ) + "}")]
    public class SnIssue : EntityBase {
        public  string      Title { get; }
        public  string      Description { get; }
        public  EntityId    ProjectId { get; }
        public  uint        IssueNumber {  get; }
        public  DateOnly    EntryDate { get; }
        public  EntityId    EnteredById { get; }
        public  EntityId    IssueTypeId {  get; }
        public  EntityId    ComponentId { get; } 
        public  EntityId    ReleaseId { get; }
        public  EntityId    WorkflowStateId { get; }
        public  EntityId    AssignedToId { get; }

        // the serializable constructor
        public SnIssue( string entityId, string title, string description, string projectId, uint issueNumber, 
                        DateOnly entryDate, string enteredById, string issueTypeId, string componentId, 
                        string releaseId, string workflowStateId, string assignedToId )
            : base( entityId ) {
            ProjectId = EntityId.CreateIdOrThrow( projectId );
            IssueTypeId = EntityId.CreateIdOrThrow( issueTypeId );
            ComponentId = EntityId.CreateIdOrThrow( componentId );
            EnteredById = EntityId.CreateIdOrThrow( enteredById );
            Title = title;
            Description = description;
            IssueNumber = issueNumber;
            EntryDate = entryDate;
            ReleaseId = EntityId.CreateIdOrThrow( releaseId );
            WorkflowStateId = EntityId.CreateIdOrThrow( workflowStateId );
            AssignedToId = EntityId.CreateIdOrThrow( assignedToId );
        }

        public SnIssue( string title, uint issueNumber, EntityId projectId ) :
            base( String.Empty ) {
            if( String.IsNullOrWhiteSpace( title )) throw new ApplicationException( "Issue titles cannot be empty" );

            Title = title;
            IssueNumber = issueNumber;
            ProjectId = EntityId.CreateIdOrThrow( projectId );

            Description = String.Empty;
            EntryDate = DateTimeProvider.Instance.CurrentDate;
            EnteredById = EntityId.Default;
            IssueTypeId = EntityId.Default;
            ComponentId = EntityId.Default;
            ReleaseId = EntityId.Default;
            WorkflowStateId = EntityId.Default;
            AssignedToId = EntityId.Default;
        }

        public SnIssue With( string ? title = null, string ? description = null, EntityId ? enteredBy = null, EntityId ? assignedTo = null ) {
            return new SnIssue( 
                EntityId,
                title ?? Title,
                description ?? Description,
                ProjectId,
                IssueNumber,
                EntryDate,
                enteredBy ?? EnteredById,
                IssueTypeId,
                ComponentId,
                ReleaseId,
                WorkflowStateId,
                assignedTo ?? AssignedToId );
        }

        public SnIssue With( SnRelease release ) {
            if( release == null ) throw new ApplicationException( "Release for issue cannot be null" );

            return new SnIssue( EntityId, Title, Description, ProjectId, IssueNumber, EntryDate, 
                                EnteredById, IssueTypeId, ComponentId, release.EntityId, WorkflowStateId, AssignedToId );
        }

        public SnIssue With( SnWorkflowState state ) {
            if( state == null ) throw new ArgumentNullException( nameof( state ), "Workflow state for issue cannot be null" );

            return new SnIssue( EntityId, Title, Description, ProjectId, IssueNumber, EntryDate, 
                                EnteredById, IssueTypeId, ComponentId, ReleaseId, state.EntityId, AssignedToId );
        }

        public SnIssue With( SnIssueType type ) {
            if( type == null ) throw new ArgumentNullException( nameof( type ), "IssueType for issue cannot be null" );

            return new SnIssue( EntityId, Title, Description, ProjectId, IssueNumber, EntryDate, 
                                EnteredById, type.EntityId, ComponentId, ReleaseId, WorkflowStateId, AssignedToId );
        }

        public SnIssue With( SnComponent component ) {
            if( component == null ) throw new ArgumentNullException( nameof( component ), "Component for issue cannot be null" );

            return new SnIssue( EntityId, Title, Description, ProjectId, IssueNumber, EntryDate, 
                                EnteredById, IssueTypeId, component.EntityId, ReleaseId, WorkflowStateId, AssignedToId );
        }
    }
}
