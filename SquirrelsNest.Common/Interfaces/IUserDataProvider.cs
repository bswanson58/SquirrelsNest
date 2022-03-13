using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.Common.Interfaces {
    public interface IUserDataProvider : IDisposable {
        Task<Either<Error, Unit>>                       SaveData( SnUserData data );

        Task<Either<Error, IEnumerable<SnUserData>>>    GetData( SnUser forUser );
        Task<Either<Error, SnUserData>>                 LoadData( SnUser forUser, UserDataType ofType );
    }
}
