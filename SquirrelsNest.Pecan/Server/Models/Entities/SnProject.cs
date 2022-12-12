using System;
using SquirrelsNest.Pecan.Server.Platform;

namespace SquirrelsNest.Pecan.Server.Models.Entities {
    public class SnProject : EntityBase {
        public string Name { get; }
        public string Description { get; }
        public DateTime Inception { get; }
        public string RepositoryUrl { get; }
        public string IssuePrefix { get; }
        public uint NextIssueNumber { get; }

        public string DebugName => $"Project: '{Name}' ({IssuePrefix})";

        public SnProject( string entityId, string dbId, string name, string description, DateTime inception, string repository, string issuePrefix,
                          uint nextIssueNumber ) :
            base( entityId, dbId ) {
            Name = name;
            Description = description;
            Inception = inception;
            RepositoryUrl = repository;
            IssuePrefix = issuePrefix;
            NextIssueNumber = nextIssueNumber;
        }

        public SnProject( string name, string issuePrefix ) :
            base( string.Empty ) {
            if( string.IsNullOrWhiteSpace( name ) ) throw new ApplicationException( "Project names cannot be empty" );
            if( string.IsNullOrWhiteSpace( issuePrefix ) ) throw new ApplicationException( "Issue Prefixes cannot be empty" );

            Name = name;
            Description = string.Empty;
            RepositoryUrl = string.Empty;
            IssuePrefix = issuePrefix;
            NextIssueNumber = 100;
            Inception = DateTimeProvider.Instance.CurrentDateTime;
        }

        public SnProject With( string? name = null, string? description = null, string? repository = null, string? issuePrefix = null ) {
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

        private static SnProject? mDefault;

        public static SnProject Default =>
            mDefault ??= new SnProject( "Unspecified", "?" );
    }
}
