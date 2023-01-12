using Fluxor;
using SquirrelsNest.Pecan.Client.Projects.Actions;
using System.Threading.Tasks;
using SquirrelsNest.Pecan.Client.Ui;

namespace SquirrelsNest.Pecan.Client.Projects.Effects {
    // ReSharper disable once UnusedType.Global
    public class WorkflowStateDeleteEffect : Effect<WorkflowStateDeleteAction> {
        private readonly IDispatcher    mDispatcher;
        private readonly UiFacade       mUiFacade;

        public WorkflowStateDeleteEffect( UiFacade uiFacade, IDispatcher dispatcher ) {
            mUiFacade = uiFacade;
            mDispatcher = dispatcher;
        }

        public override async Task HandleAsync( WorkflowStateDeleteAction action, IDispatcher dispatcher ) {
            var confirmation = await mUiFacade.ConfirmAction( "Confirm Deletion", 
                $"Would you like to delete the Workflow State named '{action.Input.WorkflowState.Name}'?" );

            if(!confirmation.Canceled ) {
                mDispatcher.Dispatch( new WorkflowStateChangeSubmitAction( action.Input ));
            } 
        }
    }
}
