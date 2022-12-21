using Fluxor;
using MudBlazor;
using SquirrelsNest.Pecan.Client.Auth.Actions;
using SquirrelsNest.Pecan.Shared.Dto.Auth;
using System.Threading.Tasks;

namespace SquirrelsNest.Pecan.Client.Auth.Effects {
    // ReSharper disable once UnusedType.Global
    public class LoginUserEffect : Effect<LoginUserAction> {
        private readonly IDialogService mDialogService;

        public LoginUserEffect( IDialogService dialogService ) {
            mDialogService = dialogService;
        }

        public override async Task HandleAsync( LoginUserAction action, IDispatcher dispatcher ) {
            var parameters = new DialogParameters { { "UserInput", new LoginUserInput() } };
            var options = new DialogOptions { FullWidth = true, CloseOnEscapeKey = true };
            var dialog = await mDialogService.ShowAsync<LoginUser>( "Login User", parameters, options );
            var dialogResult = await dialog.Result;
        
            if((!dialogResult.Cancelled ) &&
               ( dialogResult.Data is LoginUserInput userInput )) {
                dispatcher.Dispatch( new LoginUserSubmitAction( userInput ));
            }
        }
    }
}
