using Fluxor;
using SquirrelsNest.Pecan.Client.ProjectTemplates.Actions;

namespace SquirrelsNest.Pecan.Client.ProjectTemplates.Store {
    public class ProjectTemplatesFacade {
        private readonly IDispatcher    mDispatcher;

        public ProjectTemplatesFacade( IDispatcher dispatcher ) {
            mDispatcher = dispatcher;
        }

        public void RequestProjectTemplates() {
            mDispatcher.Dispatch( new RequestProjectTemplatesAction() );
        }
    }
}
