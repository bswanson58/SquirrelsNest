using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Values;

namespace SquirrelsNest.Common.Interfaces {
    public interface IProjectProvider : IEntityChangeNotifier, IDisposable {
        Task<Either<Error, SnProject>>                  AddProject( SnProject project );
        Task<Either<Error, SnProject>>                  AddProject( SnProject project, SnUser forUser );
        Task<Either<Error, Unit>>                       UpdateProject( SnProject project );
        Task<Either<Error, Unit>>                       DeleteProject( SnProject project );
        Task<Either<Error, Unit>>                       DeleteProject( SnProject project, SnUser fromUser );

        Task<Either<Error, SnProject>>                  GetProject( EntityId projectId );
        Task<Either<Error, IEnumerable<SnProject>>>     GetProjects();
        Task<Either<Error, IEnumerable<SnProject>>>     GetProjects( SnUser forUser );
    }
}
