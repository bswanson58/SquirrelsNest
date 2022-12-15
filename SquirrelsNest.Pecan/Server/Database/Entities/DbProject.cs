using System;
using SquirrelsNest.Pecan.Shared.Entities;
using SquirrelsNest.Pecan.Shared.Platform;

namespace SquirrelsNest.Pecan.Server.Database.Entities {
    public class DbProject : DbEntityBase {
        public string       Name { get; set; }
        public string       Description { get; set; }
        public DateOnly     Inception { get; set; }
        public string       RepositoryUrl { get; set; }
        public string       IssuePrefix { get; set; }
        public uint         NextIssueNumber { get; set; }

        public DbProject() {
            Name = String.Empty;
            Description = String.Empty;
            Inception = DateTimeProvider.Instance.CurrentDate;
            RepositoryUrl = String.Empty;
            IssuePrefix = String.Empty;
            NextIssueNumber = 0;
        }

        public DbProject( SnProject project ) :
            base( project.EntityId ) {
            Name = project.Name;
            Description = project.Description;
            Inception = project.Inception;
            RepositoryUrl = project.RepositoryUrl;
            IssuePrefix = project.IssuePrefix;
            NextIssueNumber = project.NextIssueNumber;
        }

        public static DbProject From( SnProject project ) => new DbProject( project );

        public SnProject ToEntity() => new SnProject( EntityId, Name, Description, Inception, RepositoryUrl, IssuePrefix, NextIssueNumber );
    }
}
