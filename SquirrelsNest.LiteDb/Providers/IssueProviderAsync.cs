using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Values;
using SquirrelsNest.LiteDb.Database;
using SquirrelsNest.LiteDb.Dto;

namespace SquirrelsNest.LiteDb.Providers {
    internal class IssueProviderAsync : IssueProvider, IIssueProvider {
        public IssueProviderAsync( IDatabaseProvider databaseProvider ) :
            base( databaseProvider ) {
        }

        public new Task<Either<Error, SnIssue>> AddIssue( SnIssue issue ) {
            return Task.Run( () => base.AddIssue( issue ));
        }

        public new Task<Either<Error, Unit>> UpdateIssue( SnIssue issue ) {
            return Task.Run( () => UpdateIssue( issue ));
        }

        public new Task<Either<Error, Unit>> DeleteIssue( SnIssue issue ) {
            return Task.Run( () => DeleteIssue( issue ));
        }

        public new Task<Either<Error, SnIssue>> GetIssue( EntityId issueId ) {
            return Task.Run( () => Get( issueId ));
        }

        public new Task<Either<Error, IEnumerable<SnIssue>>> GetIssues() {
            return Task.Run( GetEnumerable );
        }

        public new Task<Either<Error, IEnumerable<SnIssue>>> GetIssues( SnProject forProject ) {
            return Task.Run(() => GetList()
                .Map( issueList => issueList.Where( LiteDB.Query.EQ( nameof( DbIssue.ProjectId ), forProject.EntityId.Value )))
                .Map( issueList => issueList.ToEnumerable())
                .Map( entityList => from entity in entityList select entity.ToEntity()));
        }
    }
}
