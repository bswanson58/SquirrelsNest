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

        // the serializable constructor
        public SnIssue( string entityId, string dbId, string title, string description, string projectId, int issueNumber, DateOnly entryDate )
            : base( entityId, dbId ) {
            Project = CreateOrThrowId( projectId );
            Title = title;
            Description = description;
            IssueNumber = issueNumber;
            EntryDate = entryDate;
        }

        public SnIssue( string title, int issueNumber, EntityId projectId ) :
            base( String.Empty ) {
            if( String.IsNullOrWhiteSpace( title )) throw new ApplicationException( "Issue titles cannot be empty" );

            Title = title;
            IssueNumber = issueNumber;
            Project = CreateOrThrowId( projectId );

            Description = String.Empty;
            EntryDate = DateTimeProvider.Instance.CurrentDate;
        }

        public SnIssue With( string ? title = null, string ? description = null ) {
            return new SnIssue( 
                EntityId, DbId,
                title ?? Title,
                description ?? Description,
                Project,
                IssueNumber,
                EntryDate );
        }

        private EntityId CreateOrThrowId( string entityId ) {
            var id = EntityId.For( entityId );

            if( id.IsNone ) {
                throw new ApplicationException( "entity ID could not be created" );
            }

            return  id.AsEnumerable().First();
        }
    }
}
