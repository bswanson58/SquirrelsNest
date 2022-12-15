using System;
using SquirrelsNest.Pecan.Shared.Entities;
using SquirrelsNest.Pecan.Shared.Platform;

namespace SquirrelsNest.Pecan.Server.Database.Entities {
    public class DbRelease : DbEntityBase {
        public  string      ProjectId { get; set; }
        public  string      Name { get; set; }
        public  string      Description { get; set; }
        public  string      RepositoryLabel { get; set; }
        public  DateOnly    ReleaseDate { get; set; }

        public DbRelease() {
            ProjectId = String.Empty;
            Name = String.Empty;
            Description = String.Empty;
            RepositoryLabel = String.Empty;
            ReleaseDate = DateTimeProvider.Instance.CurrentDate;
        }

        public DbRelease( SnRelease release ) :
            base( release.EntityId ) {
            ProjectId = release.ProjectId;
            Name = release.Name;
            Description = release.Description;
            RepositoryLabel = release.RepositoryLabel;
            ReleaseDate = release.ReleaseDate;
        }

        public static DbRelease From( SnRelease release ) => new DbRelease( release );

        public SnRelease ToEntity() => new SnRelease( EntityId, ProjectId, Name, Description, RepositoryLabel, ReleaseDate );
    }
}
