﻿using System.Diagnostics;
using LiteDB;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Platform;

namespace SquirrelsNest.LiteDb.Dto {
    [DebuggerDisplay("{" + nameof( Name ) + "}")]
    internal class DbRelease : DbBase {
        public  string      ProjectId { get; set; }
        public  string      Name { get; set;}
        public  string      Description { get; set; }
        public  string      RepositoryLabel { get; set; }
        public  DateOnly    ReleaseDate { get; set; }

        protected DbRelease() {
            Name = String.Empty;
            Description = String.Empty;
            RepositoryLabel = String.Empty;
            ReleaseDate = DateTimeProvider.Instance.CurrentDate;
            ProjectId = Common.Values.EntityId.Default;
        }

        public static DbRelease From( SnRelease release ) {
            return new DbRelease {
                EntityId = release.EntityId,
                Id = String.IsNullOrWhiteSpace( release.DbId ) ? ObjectId.NewObjectId() : new ObjectId( release.DbId ),
                ProjectId = release.ProjectId,
                Name = release.Name,
                Description = release.Description,
                RepositoryLabel = release.RepositoryLabel,
                ReleaseDate = release.ReleaseDate
            };
        }

        public SnRelease ToEntity() {
            return new SnRelease( EntityId, Id.ToString(), ProjectId, Name, Description, RepositoryLabel, ReleaseDate );
        }
    }
}
