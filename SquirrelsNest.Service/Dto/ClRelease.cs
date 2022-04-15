using System;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Values;

namespace SquirrelsNest.Service.Dto {
    public class ClRelease : ClBase {
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        public string ProjectId { get; }
        public string Name { get; }
        public string Description { get; }
        public string RepositoryLabel { get; }
        public DateOnly ReleaseDate { get; }
        // ReSharper restore UnusedAutoPropertyAccessor.Global

        public ClRelease( string id, string projectId, string name, string description, string repositoryLabel, DateOnly releaseDate ) :
            base( id ) {
            Name = name;
            Description = description;
            ProjectId = projectId;
            RepositoryLabel = repositoryLabel;
            ReleaseDate = releaseDate;
        }

        private static ClRelease ? mDefaultRelease;

        public static ClRelease Default =>
            mDefaultRelease ??= new ClRelease( EntityId.Default.Value, EntityId.Default.Value, "Unspecified", String.Empty, string.Empty, DateOnly.MinValue );
    }

    public static class ClReleaseEx {
        public static ClRelease ToCl( this SnRelease release ) {
            return new ClRelease( release.EntityId, release.ProjectId, release.Name, release.Description, release.RepositoryLabel, release.ReleaseDate );
        }
    }

}
