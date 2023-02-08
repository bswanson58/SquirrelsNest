using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Interfaces.Database;
using SquirrelsNest.Common.Values;

namespace SquirrelsNest.Core.Database {
    internal class ComponentProvider : BaseDeleteProvider, IComponentProvider {
        private readonly IDbComponentProvider   mComponentProvider;

        public IObservable<EntitySourceChange> OnEntitySourceChange => mComponentProvider.OnEntitySourceChange;

        public ComponentProvider( IDbIssueProvider issueProvider, IDbComponentProvider componentProvider ) :
            base( issueProvider ) {
            mComponentProvider = componentProvider;
        }

        public Task<Either<Error, SnComponent>> AddComponent( SnComponent component ) => mComponentProvider.AddComponent( component );
        public Task<Either<Error, Unit>> UpdateComponent( SnComponent component ) => mComponentProvider.UpdateComponent( component );

        public async Task<Either<Error, Unit>> DeleteComponent( SnComponent component ) {
            var affected = ( await mIssueProvider.GetIssues().ConfigureAwait( false ))
                .Map( list => from i in list where i.ComponentId.Equals( component.EntityId ) select i )
                .Map( list => from i in list select i.With( SnComponent.Default ));

            return await affected
                .BindAsync( UpdateIssues )
                .BindAsync( _ => mComponentProvider.DeleteComponent( component )).ConfigureAwait( false );
        }

        public Task<Either<Error, SnComponent>> GetComponent( EntityId componentId ) => mComponentProvider.GetComponent( componentId );
        public Task<Either<Error, IEnumerable<SnComponent>>> GetComponents() => mComponentProvider.GetComponents();
        public Task<Either<Error, IEnumerable<SnComponent>>> GetComponents( SnProject forProject ) => mComponentProvider.GetComponents( forProject );

        public override void Dispose() {
            mComponentProvider.Dispose();

            base.Dispose();
        }
    }
}
