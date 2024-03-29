﻿using Fluxor;
using SquirrelsNest.Pecan.Client.Projects.Actions;
using System.Threading.Tasks;
using SquirrelsNest.Pecan.Client.Ui.Store;

namespace SquirrelsNest.Pecan.Client.Projects.Effects {
    // ReSharper disable once UnusedType.Global
    public class IssueTypeDeleteEffect : Effect<IssueTypeDeleteAction> {
        private readonly IDispatcher    mDispatcher;
        private readonly UiFacade       mUiFacade;

        public IssueTypeDeleteEffect( UiFacade uiFacade, IDispatcher dispatcher ) {
            mUiFacade = uiFacade;
            mDispatcher = dispatcher;
        }

        public override async Task HandleAsync( IssueTypeDeleteAction action, IDispatcher dispatcher ) {
            var confirmation = await mUiFacade.ConfirmAction( "Confirm Deletion",
                $"Would you like to delete the Issue Type named '{action.Input.IssueType.Name}'?" );

            if(!confirmation.Canceled ) {
                mDispatcher.Dispatch( new IssueTypeChangeSubmitAction( action.Input ));
            }
        }
    }
}
