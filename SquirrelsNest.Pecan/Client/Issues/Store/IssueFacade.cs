using Fluxor;
using SquirrelsNest.Pecan.Client.Issues.Actions;
using SquirrelsNest.Pecan.Shared.Dto.Issues;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.Issues.Store {
    public class IssueFacade {
        private readonly IDispatcher    mDispatcher;

        public IssueFacade( IDispatcher dispatcher ) {
            mDispatcher = dispatcher;
        }

        public void LoadIssues( SnCompositeProject forProject ) {
            mDispatcher.Dispatch( new LoadIssueListAction( forProject ));
        }

        public void AddIssue( SnCompositeProject forProject ) {
            mDispatcher.Dispatch( new AddIssueAction( forProject ));
        }

        public void EditIssue( SnCompositeProject forProject, SnCompositeIssue issue ) {
            mDispatcher.Dispatch( new EditIssueAction( forProject, issue ));
        }

        public void UpdateIssue( SnCompositeProject forProject, SnCompositeIssue issue ) {
            mDispatcher.Dispatch( new UpdateIssueSubmit( new UpdateIssueRequest( forProject.Project, issue )));
        }

        public void DeleteIssue( SnCompositeIssue issue ) {
            mDispatcher.Dispatch( new DeleteIssueAction( issue ));
        }

        public void EditComponent( SnCompositeProject project, SnCompositeIssue issue ) {
            mDispatcher.Dispatch( new EditComponentAction( project, issue ));
        }

        public void EditIssueType( SnCompositeProject project, SnCompositeIssue issue ) {
            mDispatcher.Dispatch( new EditIssueTypeAction( project, issue ));
        }
        public void EditWorkflowState( SnCompositeProject project, SnCompositeIssue issue ) {
            mDispatcher.Dispatch( new EditWorkflowStateAction( project, issue ));
        }
    }
}
