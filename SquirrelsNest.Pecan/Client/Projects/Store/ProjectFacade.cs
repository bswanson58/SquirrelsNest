using Fluxor;
using Microsoft.Extensions.Logging;
using SquirrelsNest.Pecan.Client.Projects.Actions;

namespace SquirrelsNest.Pecan.Client.Projects.Store
{
    public class ProjectFacade
    {
        private readonly ILogger<ProjectFacade> mLogger;
        private readonly IDispatcher mDispatcher;

        public ProjectFacade(IDispatcher dispatcher, ILogger<ProjectFacade> logger)
        {
            mDispatcher = dispatcher;
            mLogger = logger;
        }

        public void LoadProjects()
        {
            mLogger.LogInformation("Dispatching GetProjectsAction");

            mDispatcher.Dispatch(new GetProjectsAction());
        }
    }
}
