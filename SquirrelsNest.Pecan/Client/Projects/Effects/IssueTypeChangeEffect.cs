using Fluxor;
using MudBlazor;
using SquirrelsNest.Pecan.Client.Projects.Actions;
using SquirrelsNest.Pecan.Shared.Dto.Projects;
using SquirrelsNest.Pecan.Shared.Entities;
using System.Threading.Tasks;

namespace SquirrelsNest.Pecan.Client.Projects.Effects {
    // ReSharper disable once UnusedType.Global
    public class IssueTypeChangeEffect : Effect<IssueTypeChangeAction> {
        private readonly IDialogService mDialogService;

        public IssueTypeChangeEffect( IDialogService dialogService ) {
            mDialogService = dialogService;
        }

        public override async Task HandleAsync( IssueTypeChangeAction action, IDispatcher dispatcher ) {
            var parameters = new DialogParameters { { nameof( IssueTypeEditDialog.IssueType ), action.Input.IssueType } };
            var options = new DialogOptions { FullWidth = true, CloseOnEscapeKey = true };
            var dialog = await mDialogService.ShowAsync<IssueTypeEditDialog>( "Issue Type Parameters", parameters, options );
            var dialogResult = await dialog.Result;
        
            if((!dialogResult.Cancelled ) &&
               ( dialogResult.Data is SnIssueType issueType )) {
                dispatcher.Dispatch( new IssueTypeChangeSubmitAction( 
                    new IssueTypeChangeInput( issueType, action.Input.ChangeType )));
            }
        }
    }
}
