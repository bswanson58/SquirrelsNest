using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.Core.ProjectTemplates {
    public interface IProjectTemplateManager {
        IEnumerable<ProjectTemplate>    GetAvailableTemplates();
        Task<Either<Error, SnProject>>  CreateProject( ProjectTemplate fromTemplate, ProjectParameters parameters );
        Task<Either<Error, Unit>>       CreateTemplate( SnProject fromProject, TemplateParameters parameters );
    }
}
