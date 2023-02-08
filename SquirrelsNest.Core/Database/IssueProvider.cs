using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Interfaces.Database;
using SquirrelsNest.Common.Values;

namespace SquirrelsNest.Core.Database {
    internal class IssueProvider : IIssueProvider {
        private readonly IDbIssueProvider   mIssueProvider;

        public IObservable<EntitySourceChange> OnEntitySourceChange => mIssueProvider.OnEntitySourceChange;

        public IssueProvider( IDbIssueProvider issueProvider ) {
            mIssueProvider = issueProvider;
        }

        public Task<Either<Error, SnIssue>> AddIssue( SnIssue issue ) => mIssueProvider.AddIssue( issue );
        public Task<Either<Error, Unit>> UpdateIssue( SnIssue issue ) => mIssueProvider.UpdateIssue( issue );
        public Task<Either<Error, Unit>> DeleteIssue( SnIssue issue ) => mIssueProvider.DeleteIssue( issue );
        public Task<Either<Error, SnIssue>> GetIssue( EntityId issueId ) => mIssueProvider.GetIssue( issueId );
        public Task<Either<Error, IEnumerable<SnIssue>>> GetIssues() => mIssueProvider.GetIssues();
        public Task<Either<Error, IEnumerable<SnIssue>>> GetIssues( SnProject forProject ) => mIssueProvider.GetIssues( forProject );

        public void Dispose() {
            mIssueProvider.Dispose();
        }
    }
}
