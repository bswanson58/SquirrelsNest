using System.Threading.Tasks;
using MudBlazor;

namespace SquirrelsNest.Pecan.Client.Ui {
    public class UiFacade {
        private readonly IDialogService mDialogService;

        public UiFacade( IDialogService dialogService ) {
            mDialogService = dialogService;
        }

        public async Task<DialogResult> ConfirmAction( string title, string request ) {
            var parameters = new DialogParameters { { nameof( ConfirmationDialog.Request ), request } };
            var options = new DialogOptions { FullWidth = true, CloseOnEscapeKey = true };
            var dialog = await mDialogService.ShowAsync<ConfirmationDialog>( title, parameters, options );

            return await dialog.Result;
        }
    }
}
