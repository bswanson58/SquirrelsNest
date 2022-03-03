using System.Diagnostics;
using SquirrelsNest.Common.Values;

namespace SquirrelsNest.Common.Entities {
    public enum StateCategory {
        Intermediate = 0,
        Initial = 1,
        Terminal = 2,
        Completed = 3
    }

    [DebuggerDisplay("State: {" + nameof( Name ) + "}")]
    public class SnWorkflowState : EntityBase {
        public  EntityId        ProjectId { get; }
        public  string          Name { get; }
        public  string          Description { get; }
        public  StateCategory   Category { get; }

        public SnWorkflowState( string entityId, string dbId, string projectId, string name, string description, StateCategory category ) :
            base( entityId, dbId ) {
            ProjectId = EntityId.CreateIdOrThrow( projectId );
            Name = name;
            Description = description;
            Category = category;
        }

        public SnWorkflowState( string name ) :
            base( String.Empty ) {
            if( String.IsNullOrWhiteSpace( name )) throw new ApplicationException( "WorkflowState names cannot be empty" );

            ProjectId = EntityId.Default;
            Name = name;
            Description = String.Empty;
            Category = StateCategory.Initial;
        }

        public SnWorkflowState With( string ? name = null, string ? description = null, StateCategory ? category = null ) {
            return new SnWorkflowState( 
                EntityId, DbId, ProjectId,
                name ?? Name,
                description ?? Description,
                category ?? Category );
        }

        public SnWorkflowState For( SnProject project ) {
            if( project == null ) throw new ArgumentNullException( nameof( project ),  "Workflow states cannot be set to a null project" );

            return new SnWorkflowState( EntityId, DbId, project.EntityId, Name, Description, Category );
        }

        private static SnWorkflowState ? mDefaultState;

        public static SnWorkflowState Default =>
            mDefaultState ??= new SnWorkflowState( EntityId.Default, String.Empty, EntityId.Default, "Unspecified", String.Empty, StateCategory.Intermediate );
    }
}
