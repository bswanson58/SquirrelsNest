using Fluxor;
using SquirrelsNest.Pecan.Client.Projects.Actions;
using SquirrelsNest.Pecan.Client.UserData.Actions;
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

        public void EditProject( SnCompositeProject project ) {
            mDispatcher.Dispatch( new EditProjectAction( project ));
        }

        public void DeleteProject( SnCompositeProject project ) {
            mDispatcher.Dispatch( new DeleteProjectAction( project ));
        }

        public void SetCurrentProject( SnCompositeProject project ) {
            mDispatcher.Dispatch( new SetCurrentProjectAction( project ));
            mDispatcher.Dispatch( new UserDataSetCurrentProjectAction( project.EntityId ));
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

        public void AddIssueType( SnCompositeProject forProject ) {
            var issueType = new SnIssueType( forProject.Project );

            mDispatcher.Dispatch( new IssueTypeChangeAction( new IssueTypeChangeInput( issueType, EntityChangeType.Add )));
        }

        public void UpdateIssueType( SnIssueType issueType ) {
            mDispatcher.Dispatch( new IssueTypeChangeAction( new IssueTypeChangeInput( issueType, EntityChangeType.Update )));
        }

        public void DeleteIssueType( SnIssueType issueType ) {
            mDispatcher.Dispatch( new IssueTypeDeleteAction( new IssueTypeChangeInput( issueType, EntityChangeType.Delete )));
        }

        public void AddWorkflowState( SnCompositeProject forProject ) {
            var state = new SnWorkflowState( forProject.Project );

            mDispatcher.Dispatch( new WorkflowStateChangeAction( new WorkflowStateChangeInput( state, EntityChangeType.Add )));
        }

        public void UpdateWorkflowState( SnWorkflowState state ) {
            mDispatcher.Dispatch( new WorkflowStateChangeAction( new WorkflowStateChangeInput( state, EntityChangeType.Update )));
        }

        public void DeleteWorkflowState( SnWorkflowState state ) {
            mDispatcher.Dispatch( new WorkflowStateDeleteAction( new WorkflowStateChangeInput( state, EntityChangeType.Delete )));
        }
    }
}
