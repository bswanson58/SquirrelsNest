using System.Diagnostics;
using LiteDB;
using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.LiteDb.Dto {
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
                Id = String.IsNullOrWhiteSpace( component.DbId ) ? ObjectId.NewObjectId() : new ObjectId( component.DbId ),
                Name = component.Name,
                Description = component.Description
            };
        }

        public SnComponent ToEntity() {
            return new SnComponent( EntityId, Id.ToString(), ProjectId, Name, Description );
        }
    }
}
