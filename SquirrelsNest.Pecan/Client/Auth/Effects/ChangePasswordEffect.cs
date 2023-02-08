using System.Threading.Tasks;
using Fluxor;
using MudBlazor;
using SquirrelsNest.Pecan.Client.Auth.Actions;
using SquirrelsNest.Pecan.Shared.Dto.Auth;

namespace SquirrelsNest.Pecan.Client.Auth.Effects {
    // ReSharper disable once UnusedType.Global
    public class ChangePasswordEffect : Effect<ChangePasswordAction> {
        private readonly IDialogService mDialogService;

        public ChangePasswordEffect( IDialogService dialogService ) {
            mDialogService = dialogService;
        }

        public override async Task HandleAsync( ChangePasswordAction action, IDispatcher dispatcher ) {
            var parameters = new DialogParameters { { nameof( ChangePasswordDialog.Request ), action.Request } };
            var options = new DialogOptions { FullWidth = true, CloseOnEscapeKey = true };
            var dialog = await mDialogService.ShowAsync<ChangePasswordDialog>( "Update Password", parameters, options );
            var dialogResult = await dialog.Result;
        
            if((!dialogResult.Canceled ) &&
               ( dialogResult.Data is ChangePasswordRequest request )) {
                dispatcher.Dispatch( new ChangePasswordSubmit( request ));
            }
        }
    }
}
