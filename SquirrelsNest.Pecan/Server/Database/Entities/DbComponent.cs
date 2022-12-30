using System;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Server.Database.Entities {
    public class DbComponent : DbEntityBase<DbComponent> {
        public  string      ProjectId { get; set; }
        public  string      Name { get; set; }
        public  string      Description { get; set; }

        public DbComponent() {
            ProjectId = String.Empty;
            Name = String.Empty;
            Description = String.Empty;
        }

        public DbComponent( string entityId, string projectId, string name, string description ) :
            base( entityId ) {
            ProjectId = projectId;
            Name = name;
            Description = description;
        }

        public DbComponent( SnComponent component ) :
            base( component.EntityId ) {
            ProjectId = component.ProjectId;
            Name = component.Name;
            Description = component.Description;
        }

        public static DbComponent From( SnComponent component ) => new DbComponent( component );

        public SnComponent ToEntity() => new SnComponent( EntityId, ProjectId, Name, Description );

        public override void UpdateFrom( DbComponent from ) {
            ProjectId = from.ProjectId;
            Name = from.Name;
            Description = from.Description;
        }
    }
}
