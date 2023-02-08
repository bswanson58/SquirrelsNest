using System.Threading.Tasks;
using Fluxor;
using MudBlazor;
using SquirrelsNest.Pecan.Client.Issues.Actions;
using SquirrelsNest.Pecan.Client.Issues.Store;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.Issues.Effects {
    // ReSharper disable once UnusedType.Global
    public class EditAssignedUserEffect : Effect<EditAssignedUserAction> {
        private readonly IDialogService mDialogService;
        private readonly IssueFacade    mIssueFacade;

        public EditAssignedUserEffect( IDialogService dialogService, IssueFacade issueFacade ) {
            mDialogService = dialogService;
            mIssueFacade = issueFacade;
        }

        public override async Task HandleAsync( EditAssignedUserAction action, IDispatcher dispatcher ) {
            var parameters = new DialogParameters {
                { nameof( UserSelectorDialog.Project ), action.Project },
                { nameof( UserSelectorDialog.SelectedUser ), action.Issue.AssignedTo }
            };
            var options = new DialogOptions { FullWidth = true, CloseOnEscapeKey = true };

            var dialog = await mDialogService.ShowAsync<UserSelectorDialog>( "Select User", parameters, options );
            var dialogResult = await dialog.Result;

            if((!dialogResult.Canceled ) &&
               ( dialogResult.Data is SnUser user )) {
                mIssueFacade.UpdateIssue( action.Project, action.Issue.With( user ));
            }
        }
    }
}
