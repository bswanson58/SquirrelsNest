using System;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Values;

namespace SquirrelsNest.Service.Dto {
    public class ClWorkflowState : ClBase {
        public string ProjectId { get; }
        public string Name { get; }
        public string Description { get; }
        public StateCategory Category { get; }

        public ClWorkflowState( string id, string projectId, string name, string description, StateCategory category ) :
            base( id ) {
            Name = name;
            Description = description;
            ProjectId = projectId;
            Category = category;
        }

        private static ClWorkflowState ? mDefaultState;

        public static ClWorkflowState Default =>
            mDefaultState ??= new ClWorkflowState( EntityId.Default.Value, EntityId.Default.Value, "Unspecified", String.Empty, StateCategory.Initial );
    }

    public static class ClWorkflowStateEx {
        public static ClWorkflowState ToCl( this SnWorkflowState state ) {
            return new ClWorkflowState( state.EntityId, state.ProjectId, state.Name, state.Description, state.Category );
        }
    }
}
