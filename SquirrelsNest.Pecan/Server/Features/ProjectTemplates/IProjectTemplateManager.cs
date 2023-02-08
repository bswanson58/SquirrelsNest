using SquirrelsNest.Pecan.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SquirrelsNest.Pecan.Server.Features.ProjectTemplates {
    public interface IProjectTemplateManager {
        Task<IEnumerable<ProjectTemplate>>  GetAvailableTemplates();
        Task<SnProject>                     CreateProject( ProjectTemplate fromTemplate, ProjectParameters parameters );
        Task                                CreateTemplate( SnCompositeProject fromProject, TemplateParameters parameters );
    }
}
