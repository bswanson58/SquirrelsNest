using System.Linq;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.AspNetCore.Components;
using SquirrelsNest.Pecan.Client.Auth.Support;
using SquirrelsNest.Pecan.Client.Projects.Store;
using SquirrelsNest.Pecan.Client.Shared;

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

        public AppStartup( ProjectFacade projectFacade, IState<ProjectState> projectState, 
                           ITokenRefresher tokenSupport, NavigationManager navigationManager ) {
            mProjectFacade = projectFacade;
            mProjectState = projectState;
            mTokenSupport = tokenSupport;
            mNavigationManager = navigationManager;
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
            if(!mProjectState.Value.Projects.Any()) {
                mProjectFacade.LoadProjects();
            }

            mNavigationManager.NavigateTo( NavLinks.Projects );

            return Task.CompletedTask;
        }
    }
}
