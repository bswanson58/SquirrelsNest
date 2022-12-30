using Fluxor;
using MudBlazor;
using SquirrelsNest.Pecan.Client.Projects.Actions;
using SquirrelsNest.Pecan.Shared.Dto.Projects;
using SquirrelsNest.Pecan.Shared.Entities;
using System.Threading.Tasks;

namespace SquirrelsNest.Pecan.Client.Projects.Effects {
    // ReSharper disable once UnusedType.Global
    public class WorkflowStateChangeEffect : Effect<WorkflowStateChangeAction> {
        private readonly IDialogService mDialogService;

        public WorkflowStateChangeEffect( IDialogService dialogService ) {
            mDialogService = dialogService;
        }

        public override async Task HandleAsync( WorkflowStateChangeAction action, IDispatcher dispatcher ) {
            var parameters = new DialogParameters { { nameof( WorkflowStateEditDialog.WorkflowState ), action.Input.WorkflowState } };
            var options = new DialogOptions { FullWidth = true, CloseOnEscapeKey = true };
            var dialog = await mDialogService.ShowAsync<WorkflowStateEditDialog>( "Workflow State Parameters", parameters, options );
            var dialogResult = await dialog.Result;
        
            if((!dialogResult.Cancelled ) &&
               ( dialogResult.Data is SnWorkflowState workflowState )) {
                dispatcher.Dispatch( new WorkflowStateChangeSubmitAction( 
                    new WorkflowStateChangeInput( workflowState, action.Input.ChangeType )));
            }
        }
    }
}
