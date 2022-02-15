using System.Collections.Immutable;
using System.Diagnostics;
using SquirrelsNest.Common.Platform;

namespace SquirrelsNest.Common.Entities {
    [DebuggerDisplay("{" + nameof( DebugName ) + "}")]
    public class SnProject : EntityBase {
        public  string          Name { get; }
        public  string          Description { get; }
        public  DateOnly        Inception { get; }
        public  string          RepositoryUrl { get; }
        public  string          IssuePrefix { get; }
        public  int             NextIssueNumber { get; }

        public  ImmutableList<SnRelease>    Releases { get; }

        public  string      DebugName => $"Project: '{Name}' ({IssuePrefix})";

        public SnProject( string entityId, string dbId, string name, string description, DateOnly inception, string repository, string issuePrefix, int nextIssueNumber, 
                          IEnumerable<SnRelease> releases ) :
            base( entityId, dbId ) {
            Name = name;
            Description = description;
            Inception = inception;
            RepositoryUrl = repository;
            IssuePrefix = issuePrefix;
            NextIssueNumber = nextIssueNumber;
            Releases = ImmutableList.Create( releases.ToArray());
        }

        public SnProject( string name, string issuePrefix ) :
            base( String.Empty ) {
            if( String.IsNullOrWhiteSpace( name )) throw new ApplicationException( "Project names cannot be empty" );
            if( String.IsNullOrWhiteSpace( issuePrefix )) throw new ApplicationException( "Issue Prefixes cannot be empty" );

            Name = name;
            Description = String.Empty;
            RepositoryUrl = String.Empty;
            IssuePrefix = issuePrefix;
            NextIssueNumber = 100;
            Releases = ImmutableList.Create<SnRelease>();
            Inception = DateTimeProvider.Instance.CurrentDate;
        }

        public SnProject With( string ? name = null, string ? description = null, string ? repository = null, string ?  issuePrefix = null ) {
            return new SnProject( 
                EntityId, DbId,
                name ?? Name, 
                description ?? Description, 
                Inception, 
                repository ?? RepositoryUrl, 
                issuePrefix ?? IssuePrefix, 
                NextIssueNumber,
                Releases );
        }

        public SnProject WithNextIssueNumber() {
            return new SnProject( EntityId, DbId, Name, Description, Inception, RepositoryUrl, IssuePrefix, NextIssueNumber + 1, Releases );
        }

        public SnProject WithReleaseAdded( SnRelease release ) {
            if( Releases.FirstOrDefault( r => r.EntityId.Equals( release.EntityId )) != null ) throw new ApplicationException( "Release to be added exists." );

            return new SnProject( EntityId, DbId, Name, Description, Inception, RepositoryUrl, IssuePrefix, NextIssueNumber + 1, Releases.Add( release ));
        }

        public SnProject WithReleaseRemoved( SnRelease release ) {
            if(!Releases.Contains( release )) throw new ApplicationException( "Project does not contain release to be removed." );

            return new SnProject( EntityId, DbId, Name, Description, Inception, RepositoryUrl, IssuePrefix, NextIssueNumber + 1, Releases.Remove( release ));
        }

        public SnProject WithReleaseUpdated( SnRelease release ) {
            var currentValue = Releases.FirstOrDefault( r => r.EntityId.Equals( release.EntityId ));

            if( currentValue == null ) throw new ApplicationException( "Current release cannot be located for replacement" );

            return new SnProject( EntityId, DbId, Name, Description, Inception, RepositoryUrl, IssuePrefix, NextIssueNumber + 1, Releases.Replace( currentValue, release ));
        }
    }
}
