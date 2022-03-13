using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces.Database;
using SquirrelsNest.LiteDb.Database;

namespace SquirrelsNest.LiteDb.Providers {
    internal class UserDataProviderAsync : UserDataProvider, IDbUserDataProvider {
        public UserDataProviderAsync( IDatabaseProvider databaseProvider ) :
            base( databaseProvider ) {
        }

        public new Task<Either<Error, SnUserData>> AddData( SnUserData association ) {
            return Task.Run(() => base.AddData( association ));
        }

        public new Task<Either<Error, Unit>> UpdateData( SnUserData association ) {
            return Task.Run(() => base.UpdateData( association ));
        }

        public new Task<Either<Error, Unit>> DeleteData( SnUserData association ) {
            return Task.Run(() => base.DeleteData( association ));
        }

        public new Task<Either<Error, SnUserData>> GetData( SnUser forUser, UserDataType ofType ) {
            return Task.Run(() => base.GetData( forUser, ofType ));
        }

        public new Task<Either<Error, IEnumerable<SnUserData>>> GetData( SnUser forUser ) {
            return Task.Run(() => base.GetData( forUser ));
        }
    }
}
