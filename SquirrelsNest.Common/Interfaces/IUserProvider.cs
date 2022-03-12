using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Values;

namespace SquirrelsNest.Common.Interfaces {
    public interface IUserProvider : IEntityChangeNotifier, IDisposable {
        Task<Either<Error, SnUser>>                 AddUser( SnUser user );
        Task<Either<Error, Unit>>                   UpdateUser( SnUser user );
        Task<Either<Error, Unit>>                   DeleteUser( SnUser user );

        Task<Either<Error, SnUser>>                 GetUser( EntityId userId );
        Task<Either<Error, IEnumerable<SnUser>>>    GetUsers();

        Task<Either<Error, SnAssociation>>          AddAssociation( SnUser forUser, SnProject toProject );
        Task<Either<Error, Unit>>                   DeleteAssociation( SnUser fromUser, SnProject fromProject );
    }
}
