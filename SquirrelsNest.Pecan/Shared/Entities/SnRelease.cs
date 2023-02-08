using System;
using System.Diagnostics;
using System.Text.Json.Serialization;
using SquirrelsNest.Pecan.Shared.Platform;

namespace SquirrelsNest.Pecan.Shared.Entities {
    [DebuggerDisplay("Release: {" + nameof( Name ) + "}")]
    public class SnRelease : EntityBase, IComponentBase {
        public  string      ProjectId { get; }
        public  string      Name { get; }
        public  string      Description { get; }
        public  string      RepositoryLabel { get; }
        public  DateOnly    ReleaseDate { get; }

        [JsonConstructor]
        public SnRelease( string entityId, string projectId, string name, string description, string repositoryLabel, DateOnly releaseDate )
            : base( entityId ) {
            ProjectId = EntityIdentifier.CreateIdOrThrow( projectId );
            Name = name;
            Description = description;
            RepositoryLabel = repositoryLabel;
            ReleaseDate = releaseDate;
        }

        public SnRelease( string name )
            : base( String.Empty ) {
            if( String.IsNullOrWhiteSpace( name )) throw new ApplicationException( "Name names cannot be empty" );

            Name = name;

            Description = String.Empty;
            RepositoryLabel = String.Empty;
            ProjectId = EntityIdentifier.Default;
            ReleaseDate = DateTimeProvider.Instance.CurrentDate;
        }

        public SnRelease With( string ? name = null, string ? description = null, string ? repositoryLabel = null, DateOnly ? releaseDate = null ) {
            return new SnRelease( 
                    EntityId,
                    ProjectId,
                    name ?? Name,
                    description ?? Description,
                    repositoryLabel ?? RepositoryLabel,
                    releaseDate ?? ReleaseDate
                );
        }

        public SnRelease For( SnProject project ) {
            if( project == null ) throw new ArgumentNullException( nameof( project ),  "Releases cannot be set to a null project" );

            return new SnRelease( EntityId, project.EntityId, Name, Description, RepositoryLabel, ReleaseDate );
        }

        private static SnRelease ? mDefaultRelease;

        public static SnRelease Default =>
            mDefaultRelease ??= new SnRelease( EntityIdentifier.Default, EntityIdentifier.Default, "Unspecified", 
                                               String.Empty, String.Empty, DateTimeProvider.Instance.CurrentDate );

    }
}
