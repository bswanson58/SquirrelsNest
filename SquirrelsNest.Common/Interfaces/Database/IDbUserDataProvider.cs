using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.Common.Interfaces.Database {
    public interface IDbUserDataProvider : IEntityChangeNotifier, IDisposable {
        Task<Either<Error, SnUserData>>                 AddData( SnUserData data );
        Task<Either<Error, Unit>>                       DeleteData( SnUserData data );
        Task<Either<Error, Unit>>                       UpdateData( SnUserData data );

        Task<Either<Error, IEnumerable<SnUserData>>>    GetData( SnUser forUser );
        Task<Either<Error, SnUserData>>                 GetData( SnUser forUser, UserDataType ofType );
    }
}
