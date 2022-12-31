using System.Threading.Tasks;
using Fluxor;
using SquirrelsNest.Pecan.Client.Projects.Actions;
using SquirrelsNest.Pecan.Client.Ui;

namespace SquirrelsNest.Pecan.Client.Projects.Effects {
    // ReSharper disable once UnusedType.Global
    public class ComponentDeleteEffect : Effect<ComponentDeleteAction> {
        private readonly IDispatcher    mDispatcher;
        private readonly UiFacade       mUiFacade;

        public ComponentDeleteEffect( UiFacade uiFacade, IDispatcher dispatcher ) {
            mUiFacade = uiFacade;
            mDispatcher = dispatcher;
        }

        public override async Task HandleAsync( ComponentDeleteAction action, IDispatcher dispatcher ) {
            var confirmation = await mUiFacade.ConfirmAction( "Confirm Deletion", 
                $"Would you like to delete the component named '{action.Input.Component.Name}'?" );

            if(!confirmation.Cancelled ) {
                mDispatcher.Dispatch( new ComponentChangeSubmitAction( action.Input ));
            } 
        }
    }
}
