using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Values;

namespace SquirrelsNest.Common.Interfaces.Database {
    public interface IDbProjectProvider : IEntityChangeNotifier, IDisposable {
        Task<Either<Error, SnProject>>                  AddProject( SnProject project );
        Task<Either<Error, Unit>>                       UpdateProject( SnProject project );
        Task<Either<Error, Unit>>                       DeleteProject( SnProject project );

        Task<Either<Error, SnProject>>                  GetProject( EntityId projectId );
        Task<Either<Error, IEnumerable<SnProject>>>     GetProjects();
    }
}
