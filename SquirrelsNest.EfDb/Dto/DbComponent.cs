using System.Diagnostics;
using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.EfDb.Dto {
    [DebuggerDisplay("{" + nameof( Name ) + "}")]
    internal class DbComponent : DbBase {
        public  string      ProjectId { get; set; }
        public  string      Name { get; set;}
        public  string      Description { get; set; }

        protected DbComponent() {
            Name = String.Empty;
            Description = String.Empty;
            ProjectId = Common.Values.EntityId.Default;
        }

        public static DbComponent From( SnComponent component ) {
            return new DbComponent {
                EntityId = component.EntityId,
                ProjectId = component.ProjectId,
                Id = String.IsNullOrWhiteSpace( component.DbId ) ? DbIdDefault() : DbIdCreate( component.DbId ),
                Name = component.Name,
                Description = component.Description
            };
        }

        public SnComponent ToEntity() {
            return new SnComponent( EntityId, Id.ToString(), ProjectId, Name, Description );
        }
    }
}
