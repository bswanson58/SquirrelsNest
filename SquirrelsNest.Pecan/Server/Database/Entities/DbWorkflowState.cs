using System;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Server.Database.Entities {
    public class DbWorkflowState : DbEntityBase<DbWorkflowState> {
        public  string          ProjectId { get; set; }
        public  string          Name { get; set; }
        public  string          Description { get; set; }
        public  StateCategory   Category { get; set; }

        public DbWorkflowState() {
            ProjectId = String.Empty;
            Name = String.Empty;
            Description = String.Empty;
            Category = StateCategory.Initial;
        }

        public DbWorkflowState( SnWorkflowState state ) :
            base( state.EntityId ) {
            ProjectId = state.ProjectId;
            Name = state.Name;
            Description = state.Description;
            Category = state.Category;
        }

        public static DbWorkflowState From( SnWorkflowState state ) => new DbWorkflowState( state );

        public SnWorkflowState ToEntity() => new SnWorkflowState( EntityId, ProjectId, Name, Description, Category );

        public override void UpdateFrom( DbWorkflowState from ) {
            ProjectId = from.ProjectId;
            Name = from.Name;
            Description = from.Description;
            Category = from.Category;
        }
    }
}
