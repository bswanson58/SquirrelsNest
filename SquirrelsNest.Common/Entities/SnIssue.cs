using System.Diagnostics;
using SquirrelsNest.Common.Platform;
using SquirrelsNest.Common.Values;

namespace SquirrelsNest.Common.Entities {
    [DebuggerDisplay("Issue = {" + nameof( Title ) + "}")]
    public class SnIssue : EntityBase {
        public  string      Title { get; }
        public  string      Description { get; }
        public  EntityId    Project { get; }
        public  int         IssueNumber {  get; }
        public  DateOnly    EntryDate { get; }
        public  EntityId    ReleaseId { get; }

        // the serializable constructor
        public SnIssue( string entityId, string dbId, string title, string description, string projectId, int issueNumber, DateOnly entryDate, EntityId releaseId )
            : base( entityId, dbId ) {
            Project = EntityId.CreateIdOrThrow( projectId );
            Title = title;
            Description = description;
            IssueNumber = issueNumber;
            EntryDate = entryDate;
            ReleaseId = releaseId;
        }

        public SnIssue( string title, int issueNumber, EntityId projectId ) :
            base( String.Empty ) {
            if( String.IsNullOrWhiteSpace( title )) throw new ApplicationException( "Issue titles cannot be empty" );

            Title = title;
            IssueNumber = issueNumber;
            Project = EntityId.CreateIdOrThrow( projectId );

            Description = String.Empty;
            EntryDate = DateTimeProvider.Instance.CurrentDate;
            ReleaseId = EntityId.Default;
        }

        public SnIssue With( string ? title = null, string ? description = null ) {
            return new SnIssue( 
                EntityId, DbId,
                title ?? Title,
                description ?? Description,
                Project,
                IssueNumber,
                EntryDate,
                ReleaseId );
        }

        public SnIssue With( SnRelease release ) {
            if( release == null ) throw new ApplicationException( "Release for issue cannot be null" );

            return new SnIssue( EntityId, DbId, Title, Description, Project, IssueNumber, EntryDate, release.EntityId );
        }
    }
}
