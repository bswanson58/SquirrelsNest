using System;
using System.Diagnostics;
using SquirrelsNest.Pecan.Shared.Entities;
using SquirrelsNest.Pecan.Shared.Platform;

namespace SquirrelsNest.Pecan.Server.Features.Transfer.Dto {
    [DebuggerDisplay("{" + nameof( Name ) + "}")]
    internal class TrRelease : TrBase {
        public  string      ProjectId { get; set; }
        public  string      Name { get; set;}
        public  string      Description { get; set; }
        public  string      RepositoryLabel { get; set; }
        public  DateOnly    ReleaseDate { get; set; }

        public TrRelease() {
            Name = String.Empty;
            Description = String.Empty;
            RepositoryLabel = String.Empty;
            ReleaseDate = DateTimeProvider.Instance.CurrentDate;
            ProjectId = EntityIdentifier.Default;
        }

        public static TrRelease From( SnRelease release ) {
            return new TrRelease {
                EntityId = release.EntityId,
                ProjectId = release.ProjectId,
                Name = release.Name,
                Description = release.Description,
                RepositoryLabel = release.RepositoryLabel,
                ReleaseDate = release.ReleaseDate
            };
        }

        public SnRelease ToNewEntity( SnProject forProject ) {
            return new SnRelease( EntityIdentifier.CreateNew(), forProject.EntityId, Name, Description, RepositoryLabel, ReleaseDate );
        }
    }
}
