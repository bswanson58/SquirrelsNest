using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Values;
using SquirrelsNest.LiteDb.Database;

namespace SquirrelsNest.LiteDb.Providers {
    internal class ReleaseProviderAsync : ReleaseProvider, IReleaseProvider {
        public ReleaseProviderAsync( IDatabaseProvider databaseProvider ) :
            base( databaseProvider ) {
        }

        public new Task<Either<Error, SnRelease>> AddRelease( SnRelease release ) {
            return Task.Run(() => base.AddRelease( release ));
        }

        public new Task<Either<Error, Unit>> UpdateRelease( SnRelease release ) {
            return Task.Run(() => base.UpdateRelease( release ));
        }

        public new Task<Either<Error, Unit>> DeleteRelease( SnRelease release ) {
            return Task.Run(() => base.DeleteRelease( release ));
        }

        public new Task<Either<Error, SnRelease>> GetRelease( EntityId releaseId ) {
            return Task.Run(() => base.GetRelease( releaseId ));
        }

        public new Task<Either<Error, IEnumerable<SnRelease>>> GetReleases() {
            return Task.Run(() => base.GetReleases());
        }

        public new Task<Either<Error, IEnumerable<SnRelease>>> GetReleases( SnProject forProject ) {
            return Task.Run(() => base.GetReleases( forProject ));
        }
    }
}
