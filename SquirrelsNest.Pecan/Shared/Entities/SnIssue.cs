using System;
using System.Diagnostics;
using SquirrelsNest.Pecan.Shared.Platform;

namespace SquirrelsNest.Pecan.Shared.Entities {
    [DebuggerDisplay("Issue: {" + nameof( Title ) + "}")]
    public class SnIssue : EntityBase {
        public  string              Title { get; }
        public  string              Description { get; }
        public  EntityIdentifier    ProjectId { get; }
        public  uint                IssueNumber {  get; }
        public  DateOnly            EntryDate { get; }
        public  EntityIdentifier    EnteredById { get; }
        public  EntityIdentifier    IssueTypeId {  get; }
        public  EntityIdentifier    ComponentId { get; } 
        public  EntityIdentifier    ReleaseId { get; }
        public  EntityIdentifier    WorkflowStateId { get; }
        public  EntityIdentifier    AssignedToId { get; }

        // the serializable constructor
        public SnIssue( string entityId, string title, string description, string projectId, uint issueNumber, 
                        DateOnly entryDate, string enteredById, string issueTypeId, string componentId, 
                        string releaseId, string workflowStateId, string assignedToId )
            : base( entityId ) {
            ProjectId = EntityIdentifier.CreateIdOrThrow( projectId );
            IssueTypeId = EntityIdentifier.CreateIdOrThrow( issueTypeId );
            ComponentId = EntityIdentifier.CreateIdOrThrow( componentId );
            EnteredById = EntityIdentifier.CreateIdOrThrow( enteredById );
            Title = title;
            Description = description;
            IssueNumber = issueNumber;
            EntryDate = entryDate;
            ReleaseId = EntityIdentifier.CreateIdOrThrow( releaseId );
            WorkflowStateId = EntityIdentifier.CreateIdOrThrow( workflowStateId );
            AssignedToId = EntityIdentifier.CreateIdOrThrow( assignedToId );
        }

        public SnIssue( string title, string description, uint issueNumber, string projectId ) {
            if( String.IsNullOrWhiteSpace( title )) throw new ApplicationException( "Issue titles cannot be empty" );

            Title = title;
            Description = description;
            IssueNumber = issueNumber;
            ProjectId = EntityIdentifier.CreateIdOrThrow( projectId );

            EntryDate = DateTimeProvider.Instance.CurrentDate;
            EnteredById = EntityIdentifier.Default;
            IssueTypeId = EntityIdentifier.Default;
            ComponentId = EntityIdentifier.Default;
            ReleaseId = EntityIdentifier.Default;
            WorkflowStateId = EntityIdentifier.Default;
            AssignedToId = EntityIdentifier.Default;
        }

        public SnIssue With( string ? title = null, string ? description = null, 
                             SnUser ? enteredBy = null, SnUser ? assignedTo = null ) {
            return new SnIssue( 
                EntityId,
                title ?? Title,
                description ?? Description,
                ProjectId,
                IssueNumber,
                EntryDate,
                enteredBy != null ? enteredBy.EntityId : EnteredById,
                IssueTypeId,
                ComponentId,
                ReleaseId,
                WorkflowStateId,
                assignedTo != null ? assignedTo.EntityId : AssignedToId );
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
