using System.Diagnostics;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Platform;

namespace SquirrelsNest.Core.Transfer.Dto {
    [DebuggerDisplay("{" + nameof( Name ) + "}")]
    internal class TrRelease : TrBase {
        public  string      ProjectId { get; set; }
        public  string      Name { get; set;}
        public  string      Description { get; set; }
        public  string      RepositoryLabel { get; set; }
        public  DateOnly    ReleaseDate { get; set; }

        protected TrRelease() {
            Name = String.Empty;
            Description = String.Empty;
            RepositoryLabel = String.Empty;
            ReleaseDate = DateTimeProvider.Instance.CurrentDate;
            ProjectId = Common.Values.EntityId.Default;
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

        public SnRelease ToEntity() {
            return new SnRelease( EntityId, String.Empty, ProjectId, Name, Description, RepositoryLabel, ReleaseDate );
        }
    }
}
