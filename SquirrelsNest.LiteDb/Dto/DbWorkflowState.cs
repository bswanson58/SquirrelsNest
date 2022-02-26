using System.Diagnostics;
using LiteDB;
using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.LiteDb.Dto {
    [DebuggerDisplay("{" + nameof( Name ) + "}")]
    internal class DbWorkflowState : DbBase {
        public  string      ProjectId { get; set; }
        public  string      Name { get; set; }
        public  string      Description { get; set; }
        public  bool        IsInitialState { get; set; }
        public  bool        IsFinalState { get; set; }
        public  bool        IsTerminalState { get; set; }

        protected DbWorkflowState() {
            ProjectId = Common.Values.EntityId.Default;
            Name = String.Empty;
            Description = String.Empty;
            IsInitialState = false;
            IsFinalState = false;
            IsTerminalState = false;
        }

        public static DbWorkflowState From( SnWorkflowState state ) {
            return new DbWorkflowState {
                EntityId = state.EntityId,
                Id = String.IsNullOrWhiteSpace( state.DbId ) ? ObjectId.NewObjectId() : new ObjectId( state.DbId ),
                Name = state.Name,
                ProjectId = state.ProjectId,
                Description = state.Description,
                IsInitialState = state.IsInitialState,
                IsFinalState = state.IsFinalState,
                IsTerminalState = state.IsTerminalState
            };
        }

        public SnWorkflowState ToEntity() {
            return new SnWorkflowState( EntityId, Id.ToString(), ProjectId, Name, Description, IsInitialState, IsFinalState, IsTerminalState );
        }
    }
}
