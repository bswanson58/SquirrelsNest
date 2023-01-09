using System.Threading.Tasks;
using Fluxor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SquirrelsNest.Pecan.Client.Auth.Actions;
using SquirrelsNest.Pecan.Client.Auth.Support;
using SquirrelsNest.Pecan.Client.Shared;

namespace SquirrelsNest.Pecan.Client.Auth.Effects {
    // ReSharper disable once UnusedType.Global
    public class LoginUserSuccessEffect : Effect<LoginUserSuccessAction> {
        private readonly AuthenticationStateProvider    mAuthStateProvider;
        private readonly NavigationManager              mNavigationManager;

        public LoginUserSuccessEffect( AuthenticationStateProvider authStateProvider, NavigationManager navigationManager ) {
            mAuthStateProvider = authStateProvider;
            mNavigationManager = navigationManager;
        }

        public override async Task HandleAsync( LoginUserSuccessAction action, IDispatcher dispatcher ) {
            if( mAuthStateProvider is AuthStateProvider authProvider ) {
                await authProvider.SetUserAuthentication( action.UserResponse.Token, action.UserResponse.RefreshToken );

                mNavigationManager.NavigateTo( NavLinks.Projects );
            }
        }
    }
}
