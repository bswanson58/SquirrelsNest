using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Values;

namespace SquirrelsNest.Common.Interfaces {
    public interface IProjectProvider : IDisposable {
        Either<Error, SnProject>                AddProject( SnProject project );
        Either<Error, Unit>                     UpdateProject( SnProject project );
        Either<Error, Unit>                     DeleteProject( SnProject project );

        Either<Error, SnProject>                GetProject( EntityId projectId );
        Either<Error, IEnumerable<SnProject>>   GetProjects();
    }
}
