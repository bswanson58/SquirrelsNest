using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Types;

namespace SquirrelsNest.Common.Interfaces {
    public interface IProjectProvider : IDisposable {
        Either<Error, Unit>                     AddProject( SnProject project );
        Either<Error, Unit>                     UpdateProject( SnProject project );
        Either<Error, Unit>                     DeleteProject( SnProject project );

        Either<Error, SnProject>                GetProject( IssueId byId );
        Either<Error, IEnumerable<SnProject>>   GetProjects();
    }
}
