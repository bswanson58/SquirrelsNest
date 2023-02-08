using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Interfaces.Database;
using SquirrelsNest.Common.Values;

namespace SquirrelsNest.Core.Database {
    internal class IssueTypeProvider : BaseDeleteProvider, IIssueTypeProvider {
        private readonly IDbIssueTypeProvider   mIssueTypeProvider;

        public IObservable<EntitySourceChange> OnEntitySourceChange => mIssueTypeProvider.OnEntitySourceChange;

        public IssueTypeProvider( IDbIssueProvider issueProvider, IDbIssueTypeProvider issueTypeProvider ) :
            base( issueProvider ) {
            mIssueTypeProvider = issueTypeProvider;
        }

        public Task<Either<Error, SnIssueType>> AddIssue( SnIssueType issue ) => mIssueTypeProvider.AddIssue( issue );
        public Task<Either<Error, Unit>> UpdateIssue( SnIssueType issue ) => mIssueTypeProvider.UpdateIssue( issue );
        public Task<Either<Error, SnIssueType>> GetIssue( EntityId issueTypeId ) => mIssueTypeProvider.GetIssue( issueTypeId );
        public Task<Either<Error, IEnumerable<SnIssueType>>> GetIssues() => mIssueTypeProvider.GetIssues();
        public Task<Either<Error, IEnumerable<SnIssueType>>> GetIssues( SnProject forProject ) => mIssueTypeProvider.GetIssues( forProject );

        public async Task<Either<Error, Unit>> DeleteIssue( SnIssueType issue ) {
            var affected = ( await mIssueProvider.GetIssues().ConfigureAwait( false ))
                .Map( list => from i in list where i.IssueTypeId.Equals( issue.EntityId ) select i )
                .Map( list => from i in list select i.With( SnIssueType.Default ));

            return await affected
                .BindAsync( UpdateIssues )
                .BindAsync( _ => mIssueTypeProvider.DeleteIssue( issue )).ConfigureAwait( false );
        }

        public override void Dispose() {
            mIssueTypeProvider.Dispose();

            base.Dispose();
        }
    }
}
