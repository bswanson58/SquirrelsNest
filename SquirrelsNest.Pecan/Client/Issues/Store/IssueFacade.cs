using Fluxor;
using SquirrelsNest.Pecan.Client.Issues.Actions;
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

        public void DeleteIssue( SnCompositeIssue issue ) {
            mDispatcher.Dispatch( new DeleteIssueAction( issue ));
        }
    }
}
