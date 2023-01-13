using System.Threading.Tasks;
using Fluxor;
using SquirrelsNest.Pecan.Client.UserData.Actions;

namespace SquirrelsNest.Pecan.Client.UserData.Effects {
    // ReSharper disable once UnusedType.Global
    public class RequestUserDataEffect : Effect<RequestUserDataAction> {
        private readonly IUserDataService   mUserDataService;
        private readonly IDispatcher        mDispatcher;

        public RequestUserDataEffect( IUserDataService userDataService, IDispatcher dispatcher ) {
            mUserDataService = userDataService;
            mDispatcher = dispatcher;
        }

        public override async Task HandleAsync( RequestUserDataAction action, IDispatcher dispatcher ) {
            var userData = await mUserDataService.RequestUserData();

            mDispatcher.Dispatch( new RequestUserDataSuccess( userData ));
        }
    }
}
