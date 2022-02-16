using LiteDB;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Platform;

namespace SquirrelsNest.LiteDb.Dto {
    internal class DbProject : DbBase {
        public  string          Name { get; set; }
        public  string          Description { get; set; }
        public  DateOnly        Inception { get; set; }
        public  string          RepositoryUrl { get; set; }
        public  string          IssuePrefix { get; set; }
        public  int             NextIssueNumber { get; set; }

        public DbProject() {
            Name = String.Empty;
            Description = String.Empty;
            RepositoryUrl = String.Empty;
            IssuePrefix = String.Empty;
            NextIssueNumber = 1;

            Inception = DateTimeProvider.Instance.CurrentDate;
        }

        public static DbProject From( SnProject project ) {
            if( project == null ) throw new ApplicationException( "source project cannot be null" );

            return new DbProject {
                EntityId = project.EntityId,
                Id = String.IsNullOrWhiteSpace( project.DbId ) ? ObjectId.NewObjectId() : new ObjectId( project.DbId ),
                Name = project.Name,
                Description = project.Description,
                Inception = project.Inception,
                RepositoryUrl = project.RepositoryUrl,
                IssuePrefix = project.IssuePrefix,
                NextIssueNumber = project.NextIssueNumber,
            };
        }

        public SnProject ToEntity() {
            return new SnProject( EntityId, Id.ToString(), Name, Description, Inception, RepositoryUrl, IssuePrefix, NextIssueNumber );
        }
    }
}
