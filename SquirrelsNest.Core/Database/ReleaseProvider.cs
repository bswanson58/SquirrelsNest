using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Interfaces.Database;
using SquirrelsNest.Common.Values;

namespace SquirrelsNest.Core.Database {
    internal class ReleaseProvider : BaseDeleteProvider, IReleaseProvider {
        private readonly IDbReleaseProvider mReleaseProvider;

        public IObservable<EntitySourceChange> OnEntitySourceChange => mReleaseProvider.OnEntitySourceChange;

        public ReleaseProvider( IDbIssueProvider issueProvider, IDbReleaseProvider releaseProvider ) :
            base( issueProvider ) {
            mReleaseProvider = releaseProvider;
        }

        public Task<Either<Error, SnRelease>> AddRelease( SnRelease release ) => mReleaseProvider.AddRelease( release );
        public Task<Either<Error, Unit>> UpdateRelease( SnRelease release ) => mReleaseProvider.UpdateRelease( release );
        public Task<Either<Error, SnRelease>> GetRelease( EntityId releaseId ) => mReleaseProvider.GetRelease( releaseId );
        public Task<Either<Error, IEnumerable<SnRelease>>> GetReleases() => mReleaseProvider.GetReleases();
        public Task<Either<Error, IEnumerable<SnRelease>>> GetReleases( SnProject forProject ) => mReleaseProvider.GetReleases( forProject );

        public async Task<Either<Error, Unit>> DeleteRelease( SnRelease release ) {
            var affected = ( await mIssueProvider.GetIssues().ConfigureAwait( false ))
                .Map( list => from i in list where i.ReleaseId.Equals( release.EntityId ) select i )
                .Map( list => from i in list select i.With( SnRelease.Default ));

            return await affected
                .BindAsync( UpdateIssues )
                .BindAsync( _ => mReleaseProvider.DeleteRelease( release )).ConfigureAwait( false );
        }

        public override void Dispose() {
            mReleaseProvider.Dispose();

            base.Dispose();
        }
    }
}
