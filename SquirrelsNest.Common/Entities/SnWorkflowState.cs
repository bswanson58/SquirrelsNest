using System.Diagnostics;
using SquirrelsNest.Common.Values;

namespace SquirrelsNest.Common.Entities {
    [DebuggerDisplay("State: {" + nameof( Name ) + "}")]
    public class SnWorkflowState : EntityBase {
        public  EntityId    ProjectId { get; }
        public  string      Name { get; }
        public  string      Description { get; }
        public  bool        IsInitialState { get; }
        public  bool        IsFinalState { get; }
        public  bool        IsTerminalState { get; }

        public SnWorkflowState( string entityId, string dbId, string projectId, string name, string description, bool isInitial, bool isFinal, bool isTerminal ) :
            base( entityId, dbId ) {
            ProjectId = EntityId.CreateIdOrThrow( projectId );
            Name = name;
            Description = description;
            IsInitialState = isInitial;
            IsFinalState = isFinal;
            IsTerminalState = isTerminal;
        }

        public SnWorkflowState( string name ) :
            base( String.Empty ) {
            if( String.IsNullOrWhiteSpace( name )) throw new ApplicationException( "WorkflowState names cannot be empty" );

            ProjectId = EntityId.Default;
            Name = name;
            Description = String.Empty;
            IsInitialState = false;
            IsTerminalState = false;
            IsFinalState = false;
        }

        public SnWorkflowState With( string ? name = null, string ? description = null, bool ? 
                                     isInitialState = null, bool ? isFinalState = false, bool ? isTerminalState = null ) {
            return new SnWorkflowState( 
                EntityId, DbId, ProjectId,
                name ?? Name,
                description ?? Description,
                isInitialState ?? IsInitialState,
                isFinalState ?? IsFinalState,
                isTerminalState ?? IsTerminalState );
        }

        public SnWorkflowState For( SnProject project ) {
            if( project == null ) throw new ArgumentNullException( nameof( project ),  "Workflow states cannot be set to a null project" );

            return new SnWorkflowState( EntityId, DbId, project.EntityId, Name, Description, IsInitialState, IsFinalState, IsTerminalState );
        }

        private static SnWorkflowState ? mDefaultState;

        public static SnWorkflowState Default =>
            mDefaultState ??= new SnWorkflowState( EntityId.Default, String.Empty, EntityId.Default, "Unspecified", String.Empty, false, false, false );
    }
}
