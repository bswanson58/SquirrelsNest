using System.Threading.Tasks;
using Fluxor;
using MudBlazor;
using SquirrelsNest.Pecan.Client.Issues.Actions;
using SquirrelsNest.Pecan.Shared.Dto.Issues;

namespace SquirrelsNest.Pecan.Client.Issues.Effects {
    // ReSharper disable once UnusedType.Global
    public class UpdateIssueEffect : Effect<EditIssueAction> {
        private readonly IDialogService mDialogService;

        public UpdateIssueEffect( IDialogService dialogService ) {
            mDialogService = dialogService;
        }

        public override async Task HandleAsync( EditIssueAction action, IDispatcher dispatcher ) {
            var parameters = new DialogParameters {
                { nameof( UpdateIssueDialog.Issue ), new UpdateIssueRequest( action.Project.Project, action.Issue ) },
                { nameof( UpdateIssueDialog.Project ), action.Project }
            };
            var options = new DialogOptions { FullWidth = true, CloseOnEscapeKey = true };
            var dialog = await mDialogService.ShowAsync<UpdateIssueDialog>( "Issue Parameters", parameters, options );
            var dialogResult = await dialog.Result;
        
            if((!dialogResult.Canceled ) &&
               ( dialogResult.Data is UpdateIssueRequest issueRequest )) {
                dispatcher.Dispatch( new UpdateIssueSubmit( issueRequest ));
            }
        }
    }
}
