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
using SquirrelsNest.Pecan.Client.Support;
using SquirrelsNest.Pecan.Client.UserData.Store;

namespace SquirrelsNest.Pecan.Client {
    public interface IAppStartup : IDisposable {
        Task    OnStartup();
        Task    OnLogin();
        void    OnLogout();
    }

    public class AppStartup : IAppStartup {
        private readonly AuthFacade                     mAuthFacade;
        private readonly IAuthInformation               mAuthInformation;
        private readonly ProjectFacade                  mProjectFacade;
        private readonly IState<ProjectState>           mProjectState;
        private readonly NavigationManager              mNavigationManager;
        private readonly UserDataFacade                 mUserDataFacade;
        private readonly ILocalStorageService           mLocalStorage;
        private readonly ITokenExpirationChecker        mTokenChecker;

        public AppStartup( AuthFacade authFacade, ProjectFacade projectFacade, IState<ProjectState> projectState, 
                           NavigationManager navigationManager, UserDataFacade userDataFacade, 
                           ILocalStorageService localStorageService, ITokenExpirationChecker tokenChecker, 
                           IAuthInformation authInformation ) {
            mAuthFacade = authFacade;
            mProjectFacade = projectFacade;
            mProjectState = projectState;
            mNavigationManager = navigationManager;
            mUserDataFacade = userDataFacade;
            mLocalStorage = localStorageService;
            mTokenChecker = tokenChecker;
            mAuthInformation = authInformation;
        }

        public async Task OnStartup() {
            var token = await mLocalStorage.GetItemAsStringAsync( LocalStorageNames.AuthToken );
            var refresh = await mLocalStorage.GetItemAsStringAsync( LocalStorageNames.RefreshToken );

            mAuthFacade.SetInitialAuthToken( token, refresh );

            if( mAuthInformation.IsAuthValid ) {
                await OnLogin();
            }
            else {
                mNavigationManager.NavigateTo( NavLinks.Login );
            }
        }

        public Task OnLogin() {
            mUserDataFacade.RequestUserData();

            if(!mProjectState.Value.Projects.Any()) {
                mProjectFacade.LoadProjects();
            }

            mNavigationManager.NavigateTo( NavLinks.Projects );

            mTokenChecker.StartChecking();

            return Task.CompletedTask;
        }

        public void OnLogout() {
            mNavigationManager.NavigateTo( NavLinks.Home );

            mTokenChecker.StopChecking();
        }

        public void Dispose() {
            mTokenChecker.Dispose();
        }
    }
}
