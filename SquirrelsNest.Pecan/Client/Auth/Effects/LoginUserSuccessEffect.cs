using System.Threading.Tasks;
using Blazored.LocalStorage;
using Fluxor;
using Microsoft.AspNetCore.Components.Authorization;
using SquirrelsNest.Pecan.Client.Auth.Actions;
using SquirrelsNest.Pecan.Client.Auth.Support;
using SquirrelsNest.Pecan.Client.Constants;

namespace SquirrelsNest.Pecan.Client.Auth.Effects {
    // ReSharper disable once UnusedType.Global
    public class LoginUserSuccessEffect : Effect<LoginUserSuccessAction> {
        private readonly AuthenticationStateProvider    mAuthStateProvider;
        private readonly ILocalStorageService           mLocalStorage;

        public LoginUserSuccessEffect( AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage ) {
            mAuthStateProvider = authStateProvider;
            mLocalStorage = localStorage;
        }

        public override async Task HandleAsync( LoginUserSuccessAction action, IDispatcher dispatcher ) {
            await mLocalStorage.SetItemAsStringAsync( LocalStorageNames.AuthToken, action.UserResponse.Token );
            await mLocalStorage.SetItemAsStringAsync( LocalStorageNames.RefreshToken, action.UserResponse.RefreshToken );

            if( mAuthStateProvider is AuthStateProvider authProvider ) {
                authProvider.NotifyUserAuthentication( action.UserResponse.Token );
            }
        }
    }
}
