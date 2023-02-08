using System.Diagnostics;
using LiteDB;
using SquirrelsNest.Common.Entities;

// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable MemberCanBePrivate.Global

namespace SquirrelsNest.LiteDb.Dto {
    [DebuggerDisplay("{" + nameof( Name ) + "}")]
    internal class DbWorkflowState : DbBase {
        public  string          ProjectId { get; set; }
        public  string          Name { get; set; }
        public  string          Description { get; set; }
        public  StateCategory   Category { get; set; }

        protected DbWorkflowState() {
            ProjectId = Common.Values.EntityId.Default;
            Name = String.Empty;
            Description = String.Empty;
            Category = StateCategory.Intermediate;
        }

        public static DbWorkflowState From( SnWorkflowState state ) {
            return new DbWorkflowState {
                EntityId = state.EntityId,
                Id = String.IsNullOrWhiteSpace( state.DbId ) ? ObjectId.NewObjectId() : new ObjectId( state.DbId ),
                Name = state.Name,
                ProjectId = state.ProjectId,
                Description = state.Description,
                Category = state.Category
            };
        }

        public SnWorkflowState ToEntity() {
            return new SnWorkflowState( EntityId, Id.ToString(), ProjectId, Name, Description, Category );
        }
    }
}
