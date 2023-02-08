using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces.Database;
using SquirrelsNest.Common.Values;
using SquirrelsNest.LiteDb.Database;

namespace SquirrelsNest.LiteDb.Providers {
    internal class ComponentProviderAsync : ComponentProvider, IDbComponentProvider {
        public ComponentProviderAsync( IDatabaseProvider databaseProvider ) :
            base( databaseProvider ) {
        }

        public new Task<Either<Error, SnComponent>> AddComponent( SnComponent component ) {
            return Task.Run(() => base.AddComponent( component ));
        }

        public new Task<Either<Error, Unit>> UpdateComponent( SnComponent component ) {
            return Task.Run(() => base.UpdateComponent( component ));
        }

        public new Task<Either<Error, Unit>> DeleteComponent( SnComponent component ) {
            return Task.Run(() => base.DeleteComponent( component ));
        }

        public new Task<Either<Error, SnComponent>> GetComponent( EntityId componentId ) {
            return Task.Run(() => base.GetComponent( componentId ));
        }

        public new Task<Either<Error, IEnumerable<SnComponent>>> GetComponents() {
            return Task.Run(() => base.GetComponents());
        }

        public new Task<Either<Error, IEnumerable<SnComponent>>> GetComponents( SnProject forProject ) {
            return Task.Run(() => base.GetComponents( forProject ));
        }
    }
}