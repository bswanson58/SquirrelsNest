using System.Threading.Tasks;
using Fluxor;
using MudBlazor;
using SquirrelsNest.Pecan.Client.Issues.Actions;
using SquirrelsNest.Pecan.Client.Issues.Store;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.Issues.Effects {
    // ReSharper disable once UnusedType.Global
    public class EditComponentEffect : Effect<EditComponentAction> {
        private readonly IDialogService mDialogService;
        private readonly IssueFacade    mIssueFacade;

        public EditComponentEffect( IDialogService dialogService, IssueFacade issueFacade ) {
            mDialogService = dialogService;
            mIssueFacade = issueFacade;
        }

        public override async Task HandleAsync( EditComponentAction action, IDispatcher dispatcher ) {
            var parameters = new DialogParameters {
                { nameof( ComponentSelectorDialog.Project ), action.Project },
                { nameof( ComponentSelectorDialog.SelectedComponent ), action.Issue.Component }
            };
            var options = new DialogOptions { FullWidth = true, CloseOnEscapeKey = true };

            var dialog = await mDialogService.ShowAsync<ComponentSelectorDialog>( "Select Component", parameters, options );
            var dialogResult = await dialog.Result;

            if((!dialogResult.Canceled ) &&
               ( dialogResult.Data is SnComponent component )) {
                mIssueFacade.UpdateIssue( action.Project, action.Issue.With( component ));
            }
        }
    }
}
