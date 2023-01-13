using Fluxor;
using SquirrelsNest.Pecan.Client.UserData.Actions;

namespace SquirrelsNest.Pecan.Client.UserData.Store {
    public class UserDataFacade {
        private readonly IDispatcher    mDispatcher;

        public UserDataFacade( IDispatcher dispatcher ) {
            mDispatcher = dispatcher;
        }

        public void RequestUserData() {
            mDispatcher.Dispatch( new RequestUserDataAction());
        }
    }
}
