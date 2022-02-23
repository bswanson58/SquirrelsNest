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

        public  string          DebugName => $"Project: '{Name}' ({IssuePrefix})";

        public SnProject( string entityId, string dbId, string name, string description, DateOnly inception, string repository, string issuePrefix, int nextIssueNumber ) :
            base( entityId, dbId ) {
            Name = name;
            Description = description;
            Inception = inception;
            RepositoryUrl = repository;
            IssuePrefix = issuePrefix;
            NextIssueNumber = nextIssueNumber;
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
                NextIssueNumber );
        }

        public SnProject WithNextIssueNumber() {
            return new SnProject( EntityId, DbId, Name, Description, Inception, RepositoryUrl, IssuePrefix, NextIssueNumber + 1 );
        }

        private static SnProject ? mDefault;

        public static SnProject Default => 
            mDefault ??= new SnProject( "Unspecified", "?" );
    }
}
