using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces.Database;
using SquirrelsNest.Common.Values;
using SquirrelsNest.LiteDb.Database;

namespace SquirrelsNest.LiteDb.Providers {
    internal class UserProviderAsync : UserProvider, IDbUserProvider {
        public UserProviderAsync( IDatabaseProvider databaseProvider ) :
            base( databaseProvider ) {
        }

        public new Task<Either<Error, SnUser>> AddUser( SnUser release ) {
            return Task.Run(() => base.AddUser( release ));
        }

        public new Task<Either<Error, Unit>> UpdateUser( SnUser release ) {
            return Task.Run(() => base.UpdateUser( release ));
        }

        public new Task<Either<Error, Unit>> DeleteUser( SnUser release ) {
            return Task.Run(() => base.DeleteUser( release ));
        }

        public new Task<Either<Error, SnUser>> GetUser( EntityId releaseId ) {
            return Task.Run(() => base.GetUser( releaseId ));
        }

        public new Task<Either<Error, IEnumerable<SnUser>>> GetUsers() {
            return Task.Run(() => base.GetUsers());
        }
    }
}
