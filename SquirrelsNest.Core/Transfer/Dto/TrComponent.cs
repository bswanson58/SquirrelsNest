using System.Diagnostics;
using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.Core.Transfer.Dto {
    [DebuggerDisplay("{" + nameof( Name ) + "}")]
    internal class TrComponent : TrBase {
        public  string      ProjectId { get; set; }
        public  string      Name { get; set;}
        public  string      Description { get; set; }

        public TrComponent() {
            Name = String.Empty;
            Description = String.Empty;
            ProjectId = Common.Values.EntityId.Default;
        }

        public static TrComponent From( SnComponent component ) {
            return new TrComponent {
                EntityId = component.EntityId,
                ProjectId = component.ProjectId,
                Name = component.Name,
                Description = component.Description
            };
        }

        public SnComponent ToEntity() {
            return new SnComponent( EntityId, String.Empty, ProjectId, Name, Description );
        }
    }
}
