using System;
using System.Diagnostics;

namespace SquirrelsNest.Pecan.Shared.Entities {
    public enum StateCategory {
        Intermediate = 0,
        Initial = 1,
        Terminal = 2,
        Completed = 3
    }

    [DebuggerDisplay("State: {" + nameof( Name ) + "}")]
    public class SnWorkflowState : EntityBase {
        public  EntityIdentifier    ProjectId { get; }
        public  string              Name { get; }
        public  string              Description { get; }
        public  StateCategory       Category { get; }

        public SnWorkflowState( string entityId, string projectId, string name, string description, StateCategory category ) :
            base( entityId ) {
            ProjectId = EntityIdentifier.CreateIdOrThrow( projectId );
            Name = name;
            Description = description;
            Category = category;
        }

        public SnWorkflowState( string name ) :
            base( String.Empty ) {
            if( String.IsNullOrWhiteSpace( name )) throw new ApplicationException( "WorkflowState names cannot be empty" );

            ProjectId = EntityIdentifier.Default;
            Name = name;
            Description = String.Empty;
            Category = StateCategory.Initial;
        }

        public SnWorkflowState With( string ? name = null, string ? description = null, StateCategory ? category = null ) {
            return new SnWorkflowState( 
                EntityId, ProjectId,
                name ?? Name,
                description ?? Description,
                category ?? Category );
        }

        public SnWorkflowState For( SnProject project ) {
            if( project == null ) throw new ArgumentNullException( nameof( project ),  "Workflow states cannot be set to a null project" );

            return new SnWorkflowState( EntityId, project.EntityId, Name, Description, Category );
        }

        private static SnWorkflowState ? mDefaultState;

        public static SnWorkflowState Default =>
            mDefaultState ??= new SnWorkflowState( EntityIdentifier.Default, EntityIdentifier.Default, "Unspecified", String.Empty, StateCategory.Intermediate );
    }
}
