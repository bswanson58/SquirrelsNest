﻿using System.Diagnostics;
using SquirrelsNest.Common.Platform;

namespace SquirrelsNest.Common.Entities {
    [DebuggerDisplay("Release: {" + nameof( Name ) + "}")]
    public class SnRelease : EntityBase {
        public  string      ProjectId { get; }
        public  string      Name { get; }
        public  string      Description { get; }
        public  string      RepositoryLabel { get; }
        public  DateOnly    ReleaseDate { get; }

        // the serializable constructor
        public SnRelease( string entityId, string dbId, string projectId, string name, string description, string repositoryLabel, DateOnly releaseDate )
            : base( entityId, dbId ) {
            ProjectId = Values.EntityId.CreateIdOrThrow( projectId );
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
            ProjectId = Values.EntityId.Default;
            ReleaseDate = DateTimeProvider.Instance.CurrentDate;
        }

        public SnRelease With( string ? name = null, string ? description = null, string ? repositoryLabel = null, DateOnly ? releaseDate = null ) {
            return new SnRelease( 
                    EntityId, DbId,
                    ProjectId,
                    name ?? Name,
                    description ?? Description,
                    repositoryLabel ?? RepositoryLabel,
                    releaseDate ?? ReleaseDate
                );
        }

        public SnRelease For( SnProject project ) {
            if( project == null ) throw new ArgumentNullException( nameof( project ),  "Releases cannot be set to a null project" );

            return new SnRelease( EntityId, DbId, project.EntityId, Name, Description, RepositoryLabel, ReleaseDate );
        }

        private static SnRelease ? mDefaultRelease;

        public static SnRelease Default =>
            mDefaultRelease ??= new SnRelease( Values.EntityId.Default, String.Empty, Values.EntityId.Default, "Unspecified", 
                                               String.Empty, String.Empty, DateTimeProvider.Instance.CurrentDate );

    }
}
