using LanguageExt;
using LanguageExt.Common;
using SquirrelsNest.Common.Entities;

namespace SquirrelsNest.Core.ProjectTemplates {
    public interface IProjectTemplateManager {
        IEnumerable<ProjectTemplate>    GetAvailableTemplates();
        Either<Error, Unit>             CreateProject( ProjectTemplate fromTemplate, ProjectParameters parameters );
        Either<Error, Unit>             CreateTemplate( SnProject fromProject, TemplateParameters parameters );
    }
}
