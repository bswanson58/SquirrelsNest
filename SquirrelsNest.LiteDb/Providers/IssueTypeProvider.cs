using LanguageExt;
using LanguageExt.Common;
using LiteDB;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Values;
using SquirrelsNest.LiteDb.Database;
using SquirrelsNest.LiteDb.Dto;

namespace SquirrelsNest.LiteDb.Providers {
    internal class IssueTypeProvider : BaseProvider<SnIssueType, DbIssueType> {
        private static SnIssueType ConvertTo( DbIssueType issue ) => issue.ToEntity();
        private static DbIssueType ConvertFrom( SnIssueType issue ) => DbIssueType.From( issue );

        public IssueTypeProvider( IDatabaseProvider databaseProvider )
            : base( databaseProvider, DbCollectionNames.IssueTypeCollection, ConvertFrom, ConvertTo ) { }

        protected override Either<Error, LiteDatabase> InitializeDatabase( LiteDatabase db ) {
            BsonMapper.Global.Entity<DbIssueType>().Id( e => e.Id );

            return base.InitializeDatabase( db );
        }

        public Either<Error, SnIssueType> AddIssue( SnIssueType issue ) => Add( issue );
        public Either<Error, Unit> UpdateIssue( SnIssueType issue ) => Update( issue );
        public Either<Error, Unit> DeleteIssue( SnIssueType issue ) => Delete( issue );
        public Either<Error, SnIssueType> GetIssue( EntityId issueId ) => Get( issueId );
        public Either<Error, IEnumerable<SnIssueType>> GetIssues() => GetEnumerable();

        public Either<Error, IEnumerable<SnIssueType>> GetIssues( SnProject forProject ) {
            return GetList()
                .Map( issueList => issueList.Where( LiteDB.Query.EQ( nameof( DbIssueType.ProjectId ), forProject.EntityId.Value )))
                .Map( issueList => issueList.ToEnumerable())
                .Map( entityList => from entity in entityList select entity.ToEntity());
        }
    }
}