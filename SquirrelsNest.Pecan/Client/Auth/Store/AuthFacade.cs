using Fluxor;
using SquirrelsNest.Pecan.Client.Auth.Actions;

namespace SquirrelsNest.Pecan.Client.Auth.Store {
    public class AuthFacade {
        private readonly IDispatcher        mDispatcher;

        public AuthFacade( IDispatcher dispatcher ) {
            mDispatcher = dispatcher;
        }

        public void SetInitialAuthToken( string token, string refreshToken ) {
            mDispatcher.Dispatch( new SetAuthToken( token, refreshToken ));
        }

        public void RegisterUser() {
            mDispatcher.Dispatch( new CreateUserAction());
        }

        public void LoginUser() {
            mDispatcher.Dispatch( new LoginUserAction());
        }

        public void LogoutUser() {
            mDispatcher.Dispatch( new LogoutUserAction());
        }
    }
}
