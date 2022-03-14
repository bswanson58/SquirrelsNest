using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.Core.Interfaces {
    public interface IUserData {
        Task<Either<Error, T>>      Load<T>( SnUser user, UserDataType ofType ) where T : new();
        Task<Either<Error,Unit>>    Save<T>( SnUser user, UserDataType ofType, T data );
    }
}
