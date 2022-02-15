using LanguageExt;
using LanguageExt.Common;
using LiteDB;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Values;
using SquirrelsNest.LiteDb.Database;
using SquirrelsNest.LiteDb.Dto;

namespace SquirrelsNest.LiteDb.Providers {
    internal class IssueProvider : BaseProvider<SnIssue, DbIssue> {
        private static SnIssue ConvertTo( DbIssue issue ) => issue.ToEntity();
        private static DbIssue ConvertFrom( SnIssue issue ) => DbIssue.From( issue );

        public IssueProvider( IDatabaseProvider databaseProvider )
            : base( databaseProvider, DbCollectionNames.IssueCollection, ConvertFrom, ConvertTo ) { }

        protected override Either<Error, LiteDatabase> InitializeDatabase( LiteDatabase db ) {
            BsonMapper.Global.Entity<DbIssue>().Id( e => e.Id );

            return base.InitializeDatabase( db );
        }

        public Either<Error, SnIssue> AddIssue( SnIssue issue ) => Add( issue );
        public Either<Error, Unit> UpdateIssue( SnIssue issue ) => Update( issue );
        public Either<Error, Unit> DeleteIssue( SnIssue issue ) => Delete( issue );
        public Either<Error, SnIssue> GetIssue( EntityId issueId ) => Get( issueId );
        public Either<Error, IEnumerable<SnIssue>> GetIssues() => GetEnumerable();

        public Either<Error, IEnumerable<SnIssue>> GetIssues( SnProject forProject ) {
            return GetList()
                .Map( issueList => issueList.Where( LiteDB.Query.EQ( nameof( DbIssue.Project ), forProject.EntityId.Value )))
                .Map( issueList => issueList.ToEnumerable())
                .Map( entityList => from entity in entityList select entity.ToEntity());
        }
    }
}
