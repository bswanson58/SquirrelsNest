using System;
using SquirrelsNest.Pecan.Shared.Entities;
using SquirrelsNest.Pecan.Shared.Platform;

namespace SquirrelsNest.Pecan.Server.Features.Transfer.Dto {
    internal class TrProject : TrBase {
        public  string          Name { get; set; }
        public  string          Description { get; set; }
        public  DateOnly        Inception { get; set; }
        public  string          RepositoryUrl { get; set; }
        public  string          IssuePrefix { get; set; }
        public  uint            NextIssueNumber { get; set; }

        public TrProject() {
            Name = String.Empty;
            Description = String.Empty;
            RepositoryUrl = String.Empty;
            IssuePrefix = String.Empty;
            NextIssueNumber = 1;

            Inception = DateTimeProvider.Instance.CurrentDate;
        }

        public static TrProject From( SnProject project ) {
            if( project == null ) throw new ApplicationException( "source project cannot be null" );

            return new TrProject {
                EntityId = project.EntityId,
                Name = project.Name,
                Description = project.Description,
                Inception = project.Inception,
                RepositoryUrl = project.RepositoryUrl,
                IssuePrefix = project.IssuePrefix,
                NextIssueNumber = project.NextIssueNumber,
            };
        }

        public SnProject ToNewEntity() {
            return new SnProject( EntityIdentifier.CreateNew(), Name, Description, Inception, RepositoryUrl, IssuePrefix, NextIssueNumber );
        }
    }
}
