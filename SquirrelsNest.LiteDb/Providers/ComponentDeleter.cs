using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.LiteDb.Database;

namespace SquirrelsNest.LiteDb.Providers {
    internal class ComponentDeleter : ComponentProvider {
        private readonly IIssueProvider mIssueProvider;

        public ComponentDeleter( IDatabaseProvider databaseProvider, IIssueProvider issueProvider )
            : base( databaseProvider ) {
            mIssueProvider = issueProvider;
        }

        private async Task<Either<Error, Unit>> UpdateIssues( IEnumerable<SnIssue> list ) {
            foreach( var issue in list ) {
                var result = await mIssueProvider.UpdateIssue( issue ).ConfigureAwait( false );

                if( result.IsLeft ) {
                    return result;
                }
            }

            return Unit.Default;
        }

        public new async Task<Either<Error, Unit>> DeleteComponent( SnComponent component ) {
            var affected = ( await mIssueProvider.GetIssues().ConfigureAwait( false ))
                .Map( list => from i in list where i.ComponentId.Equals( component.EntityId ) select i )
                .Map( list => from i in list select i.With( SnComponent.Default ));

            return await affected.BindAsync( UpdateIssues ).ConfigureAwait( false );
        }
    }
}
