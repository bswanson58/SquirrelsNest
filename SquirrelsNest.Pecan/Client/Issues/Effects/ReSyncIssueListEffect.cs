using System.Linq;
using System.Threading.Tasks;
using Fluxor;
using SquirrelsNest.Pecan.Client.Issues.Actions;
using SquirrelsNest.Pecan.Client.Issues.Store;
using SquirrelsNest.Pecan.Client.Projects.Store;
using SquirrelsNest.Pecan.Shared.Dto;

namespace SquirrelsNest.Pecan.Client.Issues.Effects {
    // ReSharper disable once UnusedType.Global
    public class ReSyncIssueListEffect : Effect<ReSyncIssueListAction> {
        private readonly IState<ProjectState>   mProjectState;
        private readonly IState<IssueState>     mIssueState;
        private readonly IssueFacade            mIssueFacade;

        public ReSyncIssueListEffect( IState<ProjectState> projectState, IState<IssueState> issueState, 
                                      IssueFacade issueFacade ) {
            mProjectState = projectState;
            mIssueState = issueState;
            mIssueFacade = issueFacade;
        }

        public override Task HandleAsync( ReSyncIssueListAction action, IDispatcher dispatcher ) {
            var project = mProjectState.Value.Projects.FirstOrDefault( p => p.EntityId.Equals( action.Issue.ProjectId ));

            if( project != null ) {
                var request = new PageRequest( mIssueState.Value.PageInformation.CurrentPage, 
                                               mIssueState.Value.PageInformation.PageSize );

                mIssueFacade.LoadIssues( project, request );
            }

            return Task.CompletedTask;
        }
    }
}
