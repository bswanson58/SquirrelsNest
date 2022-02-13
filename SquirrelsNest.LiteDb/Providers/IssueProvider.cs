using LanguageExt;
using LanguageExt.Common;
using LiteDB;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Values;
using SquirrelsNest.LiteDb.Database;
using SquirrelsNest.LiteDb.Dto;

namespace SquirrelsNest.LiteDb.Providers {
    internal class IssueProvider : EntityProvider<DbIssue>, IIssueProvider {
        public IssueProvider( IDatabaseProvider databaseProvider )
            : base( databaseProvider, DbCollectionNames.IssueCollection ) { }

        protected override Either<Error, LiteDatabase> InitializeDatabase( LiteDatabase db ) {
            BsonMapper.Global.Entity<DbIssue>().Id( e => e.Id );

            return base.InitializeDatabase( db );
        }

        public Either<Error, SnIssue> AddIssue( SnIssue issue ) {
            return InsertEntity( DbIssue.From( issue ))
                .Map( dbIssue => dbIssue.ToEntity());
        }

        public Either<Error, Unit> UpdateIssue( SnIssue issue ) {
            return UpdateEntity( DbIssue.From( issue ));
        }

        public Either<Error, Unit> DeleteIssue( SnIssue issue ) {
            return DeleteEntity( DbIssue.From( issue ));
        }

        public Either<Error, SnIssue> GetIssue( EntityId issueId ) {
            return ValidateString( issueId )
                .Bind( _ => FindEntity( LiteDB.Query.EQ( nameof( DbIssue.EntityId ), issueId.Value )))
                .Map( dbIssue => dbIssue.ToEntity());
        }

        public Either<Error, IEnumerable<SnIssue>> GetIssues() {
            return GetList()
                .Map( issueList => issueList.ToEnumerable())
                .Map( entityList => from entity in entityList select entity.ToEntity());
        }

        public Either<Error, IEnumerable<SnIssue>> GetIssues( SnProject forProject ) {
            return GetList()
                .Map( issueList => issueList.Where( LiteDB.Query.EQ( nameof( DbIssue.Project ), forProject.EntityId.Value )))
                .Map( issueList => issueList.ToEnumerable())
                .Map( entityList => from entity in entityList select entity.ToEntity());
        }
    }
}
