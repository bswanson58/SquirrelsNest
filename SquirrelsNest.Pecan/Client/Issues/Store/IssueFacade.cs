using System.Collections.Generic;
using Fluxor;
using SquirrelsNest.Pecan.Client.Issues.Actions;
using SquirrelsNest.Pecan.Shared.Dto;
using SquirrelsNest.Pecan.Shared.Dto.Issues;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.Issues.Store {
    public class IssueFacade {
        private readonly IDispatcher    mDispatcher;

        public IssueFacade( IDispatcher dispatcher ) {
            mDispatcher = dispatcher;
        }

        public void LoadIssues( SnCompositeProject forProject, PageRequest forPage ) {
            mDispatcher.Dispatch( new LoadIssueListAction( forProject, forPage ));
        }

        public void PrepareForNewProject( string projectId, PageInformation pageInformation, uint currentDisplayPage,
                                          IEnumerable<SnCompositeIssue> issues ) {
            mDispatcher.Dispatch( new PrepareForNewProjectAction( projectId, pageInformation, currentDisplayPage, issues ));
        }

        public void SetIssueListPage( uint toPage ) {
            mDispatcher.Dispatch( new SetIssueListPageAction( toPage ));
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

        public void DeleteIssue( SnCompositeProject project, SnCompositeIssue issue ) {
            mDispatcher.Dispatch( new DeleteIssueAction( issue ));
        }

        public void ReSyncIssueList( SnCompositeIssue issue ) {
            mDispatcher.Dispatch( new ReSyncIssueListAction( issue ));
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

        public void EditAssignedUser( SnCompositeProject project, SnCompositeIssue issue ) {
            mDispatcher.Dispatch( new EditAssignedUserAction( project, issue ));
        }
    }
}
