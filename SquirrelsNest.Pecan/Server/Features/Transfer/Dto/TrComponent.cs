using System;
using System.Diagnostics;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Server.Features.Transfer.Dto {
    [DebuggerDisplay("{" + nameof( Name ) + "}")]
    internal class TrComponent : TrBase {
        public  string      ProjectId { get; set; }
        public  string      Name { get; set;}
        public  string      Description { get; set; }

        public TrComponent() {
            Name = String.Empty;
            Description = String.Empty;
            ProjectId = EntityIdentifier.Default;
        }

        public static TrComponent From( SnComponent component ) {
            return new TrComponent {
                EntityId = component.EntityId,
                ProjectId = component.ProjectId,
                Name = component.Name,
                Description = component.Description
            };
        }

        public SnComponent ToNewEntity( SnProject forProject ) {
            return new SnComponent( EntityIdentifier.CreateNew(), forProject.EntityId, Name, Description );
        }
    }
}
