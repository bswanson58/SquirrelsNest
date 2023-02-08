using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces.Database;
using SquirrelsNest.Common.Values;
using SquirrelsNest.LiteDb.Database;

namespace SquirrelsNest.LiteDb.Providers {
    internal class IssueTypeProviderAsync : IssueTypeProvider, IDbIssueTypeProvider {
        public IssueTypeProviderAsync( IDatabaseProvider databaseProvider ) :
            base( databaseProvider ) {
        }

        public new Task<Either<Error, SnIssueType>> AddIssue( SnIssueType issue ) {
            return Task.Run( () => base.AddIssue( issue ));
        }

        public new Task<Either<Error, Unit>> UpdateIssue( SnIssueType issue ) {
            return Task.Run( () => base.UpdateIssue( issue ));
        }

        public new Task<Either<Error, Unit>> DeleteIssue( SnIssueType issue ) {
            return Task.Run( () => base.DeleteIssue( issue ));
        }

        public new Task<Either<Error, SnIssueType>> GetIssue( EntityId issueId ) {
            return Task.Run( () => Get( issueId ));
        }

        public new Task<Either<Error, IEnumerable<SnIssueType>>> GetIssues() {
            return Task.Run( GetEnumerable );
        }

        public new Task<Either<Error, IEnumerable<SnIssueType>>> GetIssues( SnProject forProject ) {
            return Task.Run( () => base.GetIssues( forProject ));
        }
    }
}