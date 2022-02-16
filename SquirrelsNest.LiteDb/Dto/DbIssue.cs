using LiteDB;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Platform;

namespace SquirrelsNest.LiteDb.Dto {
    internal class DbIssue : DbBase {
        public  string      Title { get; set; }
        public  string      Description { get; set; }
        public  string      Project { get; set; }
        public  int         IssueNumber {  get; set; }
        public  DateOnly    EntryDate { get; set; }
        public  string      ReleaseId { get; set; }

        public DbIssue() {
            Title = String.Empty;
            Description = String.Empty;
            Project = String.Empty;
            IssueNumber = 0;
            EntryDate = DateTimeProvider.Instance.CurrentDate;
            ReleaseId = Common.Values.EntityId.Default;
        }

        public static DbIssue From( SnIssue issue ) {
            if( issue == null ) throw new ApplicationException( "source issue cannot be null" );

            return new DbIssue {
                EntityId = issue.EntityId,
                Id = String.IsNullOrWhiteSpace( issue.DbId ) ? ObjectId.NewObjectId() : new ObjectId( issue.DbId ),
                Title = issue.Title,
                Description = issue.Description,
                Project = issue.Project,
                IssueNumber = issue.IssueNumber,
                EntryDate = issue.EntryDate,
                ReleaseId = issue.ReleaseId
            };
        }

        public SnIssue ToEntity() {
            return new SnIssue( EntityId, Id.ToString(), Title, Description, Project, IssueNumber, EntryDate, Common.Values.EntityId.CreateIdOrThrow( ReleaseId ));
        }
    }
}
