using System.Threading.Tasks;
using Fluxor;
using MudBlazor;
using SquirrelsNest.Pecan.Client.Issues.Actions;
using SquirrelsNest.Pecan.Shared.Dto.Issues;

namespace SquirrelsNest.Pecan.Client.Issues.Effects {
    // ReSharper disable once UnusedType.Global
    public class AddIssueEffect : Effect<AddIssueAction> {
        private readonly IDialogService mDialogService;

        public AddIssueEffect( IDialogService dialogService ) {
            mDialogService = dialogService;
        }

        public override async Task HandleAsync( AddIssueAction action, IDispatcher dispatcher ) {
            var parameters = new DialogParameters {
                { nameof( CreateIssueDialog.Issue ), new CreateIssueRequest( action.Project.Project ) },
                { nameof( CreateIssueDialog.Project ), action.Project }
            };
            var options = new DialogOptions { FullWidth = true, CloseOnEscapeKey = true };
            var dialog = await mDialogService.ShowAsync<CreateIssueDialog>( "New Issue Parameters", parameters, options );
            var dialogResult = await dialog.Result;
        
            if((!dialogResult.Cancelled ) &&
               ( dialogResult.Data is CreateIssueRequest issueRequest )) {
                dispatcher.Dispatch( new AddIssueSubmitAction( issueRequest ));
            }
        }
    }
}
