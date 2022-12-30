using System.Threading.Tasks;
using Fluxor;
using MudBlazor;
using SquirrelsNest.Pecan.Client.Projects.Actions;
using SquirrelsNest.Pecan.Shared.Dto.Projects;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.Projects.Effects {
    // ReSharper disable once UnusedType.Global
    public class ComponentChangeEffect : Effect<ComponentChangeAction> {
        private readonly IDialogService mDialogService;

        public ComponentChangeEffect( IDialogService dialogService ) {
            mDialogService = dialogService;
        }

        public override async Task HandleAsync( ComponentChangeAction action, IDispatcher dispatcher ) {
            var parameters = new DialogParameters { { nameof( ComponentEditDialog.Component ), action.Input.Component } };
            var options = new DialogOptions { FullWidth = true, CloseOnEscapeKey = true };
            var dialog = await mDialogService.ShowAsync<ComponentEditDialog>( "Component Parameters", parameters, options );
            var dialogResult = await dialog.Result;
        
            if((!dialogResult.Cancelled ) &&
               ( dialogResult.Data is SnComponent component )) {
                dispatcher.Dispatch( new ComponentChangeSubmitAction( 
                    new ComponentChangeInput( component, action.Input.ChangeType )));
            }
        }
    }
}
