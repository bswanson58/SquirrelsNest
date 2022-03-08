using System.Diagnostics;
using SquirrelsNest.Common.Entities;

// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable MemberCanBePrivate.Global

namespace SquirrelsNest.Core.Transfer.Dto {
    [DebuggerDisplay("{" + nameof( Name ) + "}")]
    internal class TrWorkflowState : TrBase {
        public  string          ProjectId { get; set; }
        public  string          Name { get; set; }
        public  string          Description { get; set; }
        public  StateCategory   Category { get; set; }

        protected TrWorkflowState() {
            ProjectId = Common.Values.EntityId.Default;
            Name = String.Empty;
            Description = String.Empty;
            Category = StateCategory.Intermediate;
        }

        public static TrWorkflowState From( SnWorkflowState state ) {
            return new TrWorkflowState {
                EntityId = state.EntityId,
                Name = state.Name,
                ProjectId = state.ProjectId,
                Description = state.Description,
                Category = state.Category
            };
        }

        public SnWorkflowState ToEntity() {
            return new SnWorkflowState( EntityId, String.Empty, ProjectId, Name, Description, Category );
        }
    }
}
