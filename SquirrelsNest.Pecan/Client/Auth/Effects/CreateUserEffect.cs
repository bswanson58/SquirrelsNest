using Fluxor;
using MudBlazor;
using SquirrelsNest.Pecan.Shared.Dto;
using System.Threading.Tasks;
using SquirrelsNest.Pecan.Client.Auth.Actions;

namespace SquirrelsNest.Pecan.Client.Auth.Effects {
    // ReSharper disable once UnusedType.Global
    public class CreateUserEffect : Effect<CreateUserAction> {
        private readonly IDialogService mDialogService;

        public CreateUserEffect( IDialogService dialogService ) {
            mDialogService = dialogService;
        }

        public override async Task HandleAsync( CreateUserAction action, IDispatcher dispatcher ) {
            var parameters = new DialogParameters { { "user", new CreateUserInput() } };
            var options = new DialogOptions { FullWidth = true, CloseOnEscapeKey = true };
            var dialog = await mDialogService.ShowAsync<CreateUserDialog>( "Register User", parameters, options );
            var dialogResult = await dialog.Result;
        
            if((!dialogResult.Cancelled ) &&
               ( dialogResult.Data is CreateUserInput userInput )) {
                dispatcher.Dispatch( new CreateUserSubmitAction( userInput ));
            }
        }
    }
}
