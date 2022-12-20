using Fluxor;
using SquirrelsNest.Pecan.Client.Auth.Actions;

namespace SquirrelsNest.Pecan.Client.Auth.Store {
    public class AuthFacade {
        private readonly IDispatcher mDispatcher;

        public AuthFacade( IDispatcher dispatcher ) {
            mDispatcher = dispatcher;
        }

        public void RegisterUser() {
            mDispatcher.Dispatch( new CreateUserAction());
        }
    }
}
