using Fluxor;
using Microsoft.AspNetCore.Components;
using SquirrelsNest.Pecan.Client.Auth.Actions;
using SquirrelsNest.Pecan.Client.Shared;
using SquirrelsNest.Pecan.Shared.Dto.Auth;

namespace SquirrelsNest.Pecan.Client.Auth.Store {
    public class AuthFacade {
        private readonly IDispatcher        mDispatcher;
        private readonly NavigationManager  mNavigationManager;

        public AuthFacade( IDispatcher dispatcher, NavigationManager navigationManager ) {
            mDispatcher = dispatcher;
            mNavigationManager = navigationManager;
        }

        public void SetInitialAuthToken( string token, string refreshToken ) {
            mDispatcher.Dispatch( new SetAuthToken( token, refreshToken ));
        }

        public void RegisterUser() {
            mDispatcher.Dispatch( new CreateUserAction());
        }

        public void LoginUser() {
            mNavigationManager.NavigateTo( NavLinks.Login );
        }

        public void LoginUser( LoginUserInput input ) {
            mDispatcher.Dispatch( new LoginUserSubmitAction( input ));
        }

        public void LogoutUser() {
            mDispatcher.Dispatch( new LogoutUserAction());
        }
    }
}
