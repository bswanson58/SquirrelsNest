using Fluxor;
using SquirrelsNest.Pecan.Client.Users.Actions;

namespace SquirrelsNest.Pecan.Client.Users.Store {
    public class UserFacade {
        private readonly IDispatcher    mDispatcher;

        public UserFacade( IDispatcher dispatcher ) {
            mDispatcher = dispatcher;
        }

        public void LoadUsers() {
            mDispatcher.Dispatch( new GetUsersAction());
        }
    }
}
