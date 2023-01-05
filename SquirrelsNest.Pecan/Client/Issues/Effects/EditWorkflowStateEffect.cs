using Fluxor;
using MudBlazor;
using SquirrelsNest.Pecan.Client.Issues.Actions;
using SquirrelsNest.Pecan.Client.Issues.Store;
using SquirrelsNest.Pecan.Shared.Entities;
using System.Threading.Tasks;

namespace SquirrelsNest.Pecan.Client.Issues.Effects {
    // ReSharper disable once UnusedType.Global
    public class EditWorkflowStateEffect : Effect<EditWorkflowStateAction> {
        private readonly IDialogService mDialogService;
        private readonly IssueFacade    mIssueFacade;

        public EditWorkflowStateEffect( IDialogService dialogService, IssueFacade issueFacade ) {
            mDialogService = dialogService;
            mIssueFacade = issueFacade;
        }

        public override async Task HandleAsync( EditWorkflowStateAction action, IDispatcher dispatcher ) {
            var parameters = new DialogParameters {
                { nameof( WorkflowStateSelectorDialog.Project ), action.Project },
                { nameof( WorkflowStateSelectorDialog.SelectedWorkflowState ), action.Issue.WorkflowState }
            };
            var options = new DialogOptions { FullWidth = true, CloseOnEscapeKey = true };

            var dialog = await mDialogService.ShowAsync<WorkflowStateSelectorDialog>( "Select Workflow State", parameters, options );
            var dialogResult = await dialog.Result;

            if((!dialogResult.Cancelled ) &&
               ( dialogResult.Data is SnWorkflowState state )) {
                mIssueFacade.UpdateIssue( action.Project, action.Issue.With( state ));
            }
        }
    }
}
