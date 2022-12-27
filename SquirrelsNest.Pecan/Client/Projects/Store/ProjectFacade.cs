using Fluxor;
using Microsoft.Extensions.Logging;
using SquirrelsNest.Pecan.Client.Projects.Actions;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.Projects.Store {
    public class ProjectFacade {
        private readonly ILogger<ProjectFacade> mLogger;
        private readonly IDispatcher mDispatcher;

        public ProjectFacade( IDispatcher dispatcher, ILogger<ProjectFacade> logger ) {
            mDispatcher = dispatcher;
            mLogger = logger;
        }

        public void LoadProjects() {
            mDispatcher.Dispatch( new GetProjectsAction());
        }

        public void AddProject() {
            mDispatcher.Dispatch( new AddProjectAction());
        }

        public void SetCurrentProject( SnProject project ) {
            mDispatcher.Dispatch( new SetCurrentProjectAction( project ));
        }
    }
}
