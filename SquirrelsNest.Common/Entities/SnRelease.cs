using System.Diagnostics;
using SquirrelsNest.Common.Platform;

namespace SquirrelsNest.Common.Entities {
    [DebuggerDisplay("Release = {" + nameof( Version ) + "}")]
    public class SnRelease : EntityBase {
        public  string      Version { get; }
        public  string      Description { get; }
        public  string      RepositoryLabel { get; }
        public  DateOnly    ReleaseDate { get; }

        // the serializable constructor
        public SnRelease( string entityId, string dbId, string version, string description, string repositoryLabel, DateOnly releaseDate )
            : base( entityId, dbId ) {
            Version = version;
            Description = description;
            RepositoryLabel = repositoryLabel;
            ReleaseDate = releaseDate;
        }

        public SnRelease( string version )
            : base( String.Empty ) {
            if( String.IsNullOrWhiteSpace( version )) throw new ApplicationException( "Version names cannot be empty" );

            Version = version;

            Description = String.Empty;
            RepositoryLabel = String.Empty;

            ReleaseDate = DateTimeProvider.Instance.CurrentDate;
        }

        public SnRelease With( string ? version = null, string ? description = null, string ? repositoryLabel = null, DateOnly ? releaseDate = null ) {
            return new SnRelease( 
                    EntityId, DbId,
                    version ?? Version,
                    description ?? Description,
                    repositoryLabel ?? RepositoryLabel,
                    releaseDate ?? ReleaseDate
                );
        }
    }
}
