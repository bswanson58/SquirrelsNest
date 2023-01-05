using System.Threading.Tasks;
using Fluxor;
using MudBlazor;
using SquirrelsNest.Pecan.Client.Issues.Actions;
using SquirrelsNest.Pecan.Shared.Dto.Issues;

namespace SquirrelsNest.Pecan.Client.Issues.Effects {
    // ReSharper disable once UnusedType.Global
    public class EditIssueEffect : Effect<EditIssueAction> {
        private readonly IDialogService mDialogService;

        public EditIssueEffect( IDialogService dialogService ) {
            mDialogService = dialogService;
        }

        public override async Task HandleAsync( EditIssueAction action, IDispatcher dispatcher ) {
            var parameters = new DialogParameters {
                { nameof( IssueEditDialog.Issue ), new CreateIssueRequest( action.Project.Project ) },
                { nameof( IssueEditDialog.Project ), action.Project }
            };
            var options = new DialogOptions { FullWidth = true, CloseOnEscapeKey = true };
            var dialog = await mDialogService.ShowAsync<IssueEditDialog>( "Issue Parameters", parameters, options );
            var dialogResult = await dialog.Result;
        
            if((!dialogResult.Cancelled ) &&
               ( dialogResult.Data is CreateIssueRequest issueRequest )) {
                dispatcher.Dispatch( new AddIssueSubmitAction( issueRequest ));
            }
        }
    }
}
