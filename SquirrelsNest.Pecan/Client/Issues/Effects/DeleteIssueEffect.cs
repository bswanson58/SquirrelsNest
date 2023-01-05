using System.Threading.Tasks;
using Fluxor;
using SquirrelsNest.Pecan.Client.Issues.Actions;
using SquirrelsNest.Pecan.Client.Ui;

namespace SquirrelsNest.Pecan.Client.Issues.Effects {
    // ReSharper disable once UnusedType.Global
    public class DeleteIssueEffect : Effect<DeleteIssueAction> {
        private readonly IDispatcher    mDispatcher;
        private readonly UiFacade       mUiFacade;

        public DeleteIssueEffect( IDispatcher dispatcher, UiFacade uiFacade ) {
            mDispatcher = dispatcher;
            mUiFacade = uiFacade;
        }

        public override async Task HandleAsync( DeleteIssueAction action, IDispatcher dispatcher ) {
            var confirmation = await mUiFacade.ConfirmAction( "Confirm Deletion", 
                $"Would you like to delete the Issue titled '{action.Issue.Title}'?" );

            if(!confirmation.Cancelled ) {
//                mDispatcher.Dispatch( new IssueTypeChangeSubmitAction( action.Input ));
            } 
        }
    }
}
