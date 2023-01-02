using Fluxor;
using SquirrelsNest.Pecan.Client.Issues.Actions;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.Issues.Store {
    public class IssueFacade {
        private readonly IDispatcher    mDispatcher;

        public IssueFacade( IDispatcher dispatcher ) {
            mDispatcher = dispatcher;
        }

        public void AddIssue( SnCompositeProject forProject ) {
            mDispatcher.Dispatch( new AddIssueAction( forProject ));
        }
    }
}
