using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Values;

namespace SquirrelsNest.Common.Interfaces {
    public interface IReleaseProvider : IEntityChangeNotifier, IDisposable {
        Task<Either<Error, SnRelease>>                  AddRelease( SnRelease release );
        Task<Either<Error, Unit>>                       UpdateRelease( SnRelease release );
        Task<Either<Error, Unit>>                       DeleteRelease( SnRelease release );

        Task<Either<Error, SnRelease>>                  GetRelease( EntityId releaseId );
        Task<Either<Error, IEnumerable<SnRelease>>>     GetReleases();
    }
}
