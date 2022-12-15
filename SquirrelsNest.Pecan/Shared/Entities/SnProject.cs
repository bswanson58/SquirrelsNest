using System;
using System.Diagnostics;
using System.Text.Json.Serialization;
using SquirrelsNest.Pecan.Shared.Platform;

namespace SquirrelsNest.Pecan.Shared.Entities {
    [DebuggerDisplay("{" + nameof( DebugName ) + "}")]
    public class SnProject : EntityBase {
        public string       Name { get; }
        public string       Description { get; }
        public DateOnly     Inception { get; }
        public string       RepositoryUrl { get; }
        public string       IssuePrefix { get; }
        public uint         NextIssueNumber { get; }

        public string       DebugName => $"Project: ({IssuePrefix}) '{Name}'";

        [JsonConstructor]
        public SnProject( string entityId, string name, string description, DateOnly inception, 
                          string repositoryUrl, string issuePrefix, uint nextIssueNumber ) :
            base( entityId ) {
            Name = name;
            Description = description;
            Inception = inception;
            RepositoryUrl = repositoryUrl;
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
            Inception = DateTimeProvider.Instance.CurrentDate;
        }

        public SnProject With( string? name = null, string? description = null, string? repository = null, string? issuePrefix = null ) {
            return new SnProject(
                EntityId,
                name ?? Name,
                description ?? Description,
                Inception,
                repository ?? RepositoryUrl,
                issuePrefix ?? IssuePrefix,
                NextIssueNumber );
        }

        public SnProject WithNextIssueNumber() {
            return new SnProject( EntityId, Name, Description, Inception, RepositoryUrl, IssuePrefix, NextIssueNumber + 1 );
        }

        private static SnProject? mDefault;

        public static SnProject Default =>
            mDefault ??= new SnProject( "Unspecified", "?" );
    }
}
