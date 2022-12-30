using Fluxor;
using SquirrelsNest.Pecan.Client.Projects.Actions;
using SquirrelsNest.Pecan.Shared.Dto.Projects;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.Projects.Store {
    public class ProjectFacade {
        private readonly IDispatcher    mDispatcher;

        public ProjectFacade( IDispatcher dispatcher ) {
            mDispatcher = dispatcher;
        }

        public void LoadProjects() {
            mDispatcher.Dispatch( new GetProjectsAction());
        }

        public void AddProject() {
            mDispatcher.Dispatch( new AddProjectAction());
        }

        public void SetCurrentProject( SnCompositeProject project ) {
            mDispatcher.Dispatch( new SetCurrentProjectAction( project ));
        }

        public void AddComponent( SnCompositeProject forProject ) {
            var component = new SnComponent( forProject.Project );

            mDispatcher.Dispatch( new ComponentChangeAction( new ComponentChangeInput( component, EntityChangeType.Add )));
        }

        public void UpdateComponent( SnComponent component ) {
            mDispatcher.Dispatch( new ComponentChangeAction( new ComponentChangeInput( component, EntityChangeType.Update )));
        }

        public void DeleteComponent( SnComponent component ) {
            mDispatcher.Dispatch( new ComponentDeleteAction( new ComponentChangeInput( component, EntityChangeType.Delete )));
        }
    }
}
