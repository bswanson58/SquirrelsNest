using System.Threading.Tasks;
using Fluxor;
using MudBlazor;
using SquirrelsNest.Pecan.Client.Issues.Actions;
using SquirrelsNest.Pecan.Client.Issues.Store;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.Issues.Effects {
    // ReSharper disable once UnusedType.Global
    public class EditIssueTypeEffect : Effect<EditIssueTypeAction> {
        private readonly IDialogService mDialogService;
        private readonly IssueFacade    mIssueFacade;

        public EditIssueTypeEffect( IDialogService dialogService, IssueFacade issueFacade ) {
            mDialogService = dialogService;
            mIssueFacade = issueFacade;
        }

        public override async Task HandleAsync( EditIssueTypeAction action, IDispatcher dispatcher ) {
            var parameters = new DialogParameters {
                { nameof( IssueTypeSelectorDialog.Project ), action.Project },
                { nameof( IssueTypeSelectorDialog.SelectedIssueType ), action.Issue.IssueType }
            };
            var options = new DialogOptions { FullWidth = true, CloseOnEscapeKey = true };

            var dialog = await mDialogService.ShowAsync<IssueTypeSelectorDialog>( "Select Issue Type", parameters, options );
            var dialogResult = await dialog.Result;

            if((!dialogResult.Cancelled ) &&
               ( dialogResult.Data is SnIssueType type )) {
                mIssueFacade.UpdateIssue( action.Project, action.Issue.With( type ));
            }
        }
    }
}
