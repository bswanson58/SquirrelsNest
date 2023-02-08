using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces.Database;

namespace SquirrelsNest.Core.Database {
    internal class BaseDeleteProvider : IDisposable {
        protected readonly IDbIssueProvider mIssueProvider;

        protected BaseDeleteProvider( IDbIssueProvider issueProvider ) {
            mIssueProvider = issueProvider;
        }

        protected async Task<Either<Error, Unit>> UpdateIssues( IEnumerable<SnIssue> list ) {
            foreach( var issue in list ) {
                var result = await mIssueProvider.UpdateIssue( issue ).ConfigureAwait( false );

                if( result.IsLeft ) {
                    return result;
                }
            }

            return Unit.Default;
        }

        public virtual void Dispose() {
            mIssueProvider.Dispose();
        }
    }
}
