using System.Threading.Tasks;
using Fluxor;
using SquirrelsNest.Pecan.Client.Issues.Actions;
using SquirrelsNest.Pecan.Client.Issues.Store;

namespace SquirrelsNest.Pecan.Client.Issues.Effects {
    // ReSharper disable once UnusedType.Global
    public class DeleteIssueSuccessEffect : Effect<DeleteIssueSuccess> {
        private readonly IState<IssueState>     mIssueState;
        private readonly IssueFacade            mIssueFacade;

        public DeleteIssueSuccessEffect( IState<IssueState> issueState, IssueFacade issueFacade ) {
            mIssueState = issueState;
            mIssueFacade = issueFacade;
        }

        public override Task HandleAsync( DeleteIssueSuccess action, IDispatcher dispatcher ) {
            if( mIssueState.Value.PageInformation.HasNext ) {
                mIssueFacade.ReSyncIssueList( action.Issue );
            }

            return Task.CompletedTask;
        }
    }
}
