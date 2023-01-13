using System.Linq;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.AspNetCore.Components;
using SquirrelsNest.Pecan.Client.Auth.Support;
using SquirrelsNest.Pecan.Client.Projects.Store;
using SquirrelsNest.Pecan.Client.Shared;
using SquirrelsNest.Pecan.Client.UserData.Store;

namespace SquirrelsNest.Pecan.Client {
    public interface IAppStartup {
        Task    OnStartup();
        Task    OnLogin();
    }

    public class AppStartup : IAppStartup {
        private readonly ProjectFacade          mProjectFacade;
        private readonly IState<ProjectState>   mProjectState;
        private readonly ITokenRefresher        mTokenSupport;
        private readonly NavigationManager      mNavigationManager;
        private readonly UserDataFacade         mUserDataFacade;

        public AppStartup( ProjectFacade projectFacade, IState<ProjectState> projectState, 
                           ITokenRefresher tokenSupport, NavigationManager navigationManager,
                           UserDataFacade userDataFacade ) {
            mProjectFacade = projectFacade;
            mProjectState = projectState;
            mTokenSupport = tokenSupport;
            mNavigationManager = navigationManager;
            mUserDataFacade = userDataFacade;
        }

        public async Task OnStartup() {
            if( await mTokenSupport.TokenRefreshRequired( 1 )) {
                mNavigationManager.NavigateTo( NavLinks.Login );
            }
            else {
                await OnLogin();
            }
        }

        public Task OnLogin() {
            mUserDataFacade.RequestUserData();

            if(!mProjectState.Value.Projects.Any()) {
                mProjectFacade.LoadProjects();
            }

            mNavigationManager.NavigateTo( NavLinks.Projects );

            return Task.CompletedTask;
        }
    }
}
