using System;
using System.Diagnostics;
using SquirrelsNest.Pecan.Shared.Entities;

// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable MemberCanBePrivate.Global

namespace SquirrelsNest.Pecan.Server.Features.Transfer.Dto {
    [DebuggerDisplay("{" + nameof( Name ) + "}")]
    internal class TrWorkflowState : TrBase {
        public  string          ProjectId { get; set; }
        public  string          Name { get; set; }
        public  string          Description { get; set; }
        public  StateCategory   Category { get; set; }

        public TrWorkflowState() {
            ProjectId = EntityIdentifier.Default;
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

        public SnWorkflowState ToNewEntity( SnProject forProject ) {
            return new SnWorkflowState( EntityIdentifier.CreateNew(), forProject.EntityId, Name, Description, Category );
        }
    }
}
