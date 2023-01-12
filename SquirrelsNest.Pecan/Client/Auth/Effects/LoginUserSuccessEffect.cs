using System.Threading.Tasks;
using Fluxor;
using Microsoft.AspNetCore.Components.Authorization;
using SquirrelsNest.Pecan.Client.Auth.Actions;
using SquirrelsNest.Pecan.Client.Auth.Support;

namespace SquirrelsNest.Pecan.Client.Auth.Effects {
    // ReSharper disable once UnusedType.Global
    public class LoginUserSuccessEffect : Effect<LoginUserSuccessAction> {
        private readonly AuthenticationStateProvider    mAuthStateProvider;
        private readonly IAppStartup                    mAppStartup;

        public LoginUserSuccessEffect( AuthenticationStateProvider authStateProvider, IAppStartup appStartup ) {
            mAuthStateProvider = authStateProvider;
            mAppStartup = appStartup;
        }

        public override async Task HandleAsync( LoginUserSuccessAction action, IDispatcher dispatcher ) {
            if( mAuthStateProvider is AuthStateProvider authProvider ) {
                await authProvider.SetUserAuthentication( action.UserResponse.Token, action.UserResponse.RefreshToken );

                await mAppStartup.OnLogin();
            }
        }
    }
}
