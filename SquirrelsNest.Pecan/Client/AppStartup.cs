using System;
using System.Linq;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Fluxor;
using Microsoft.AspNetCore.Components;
using SquirrelsNest.Pecan.Client.Auth.Store;
using SquirrelsNest.Pecan.Client.Auth.Support;
using SquirrelsNest.Pecan.Client.Constants;
using SquirrelsNest.Pecan.Client.Projects.Store;
using SquirrelsNest.Pecan.Client.Shared;
using SquirrelsNest.Pecan.Client.UserData.Store;

namespace SquirrelsNest.Pecan.Client {
    public interface IAppStartup {
        Task    OnStartup();
        Task    OnLogin();
    }

    public class AppStartup : IAppStartup {
        private readonly AuthFacade             mAuthFacade;
        private readonly ProjectFacade          mProjectFacade;
        private readonly IState<ProjectState>   mProjectState;
        private readonly ITokenRefresher        mTokenSupport;
        private readonly NavigationManager      mNavigationManager;
        private readonly UserDataFacade         mUserDataFacade;
        private readonly ILocalStorageService   mLocalStorage;

        public AppStartup( AuthFacade authFacade, ProjectFacade projectFacade, IState<ProjectState> projectState, 
                           ITokenRefresher tokenSupport, NavigationManager navigationManager,
                           UserDataFacade userDataFacade, ILocalStorageService localStorageService ) {
            mAuthFacade = authFacade;
            mProjectFacade = projectFacade;
            mProjectState = projectState;
            mTokenSupport = tokenSupport;
            mNavigationManager = navigationManager;
            mUserDataFacade = userDataFacade;
            mLocalStorage = localStorageService;
        }

        public async Task OnStartup() {
            var token = await mLocalStorage.GetItemAsStringAsync( LocalStorageNames.AuthToken );
            var refresh = await mLocalStorage.GetItemAsStringAsync( LocalStorageNames.RefreshToken );

            if(!String.IsNullOrWhiteSpace( token )) {
                mAuthFacade.SetInitialAuthToken( token, refresh );
            }

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
