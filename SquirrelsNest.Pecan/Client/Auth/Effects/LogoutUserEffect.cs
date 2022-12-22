using System.Net.Http;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Fluxor;
using Microsoft.AspNetCore.Components.Authorization;
using SquirrelsNest.Pecan.Client.Auth.Actions;
using SquirrelsNest.Pecan.Client.Auth.Support;

namespace SquirrelsNest.Pecan.Client.Auth.Effects {
    // ReSharper disable once UnusedType.Global
    public class LogoutUserEffect : Effect<LogoutUserAction> {
        private readonly HttpClient                     mHttpClient;
        private readonly AuthenticationStateProvider    mAuthStateProvider;
        private readonly ILocalStorageService           mLocalStorage;

        public LogoutUserEffect( HttpClient httpClient, AuthenticationStateProvider authStateProvider, 
                                 ILocalStorageService localStorage ) {
            mHttpClient = httpClient;
            mAuthStateProvider = authStateProvider;
            mLocalStorage = localStorage;
        }

        public override async Task HandleAsync( LogoutUserAction action, IDispatcher dispatcher ) {
            await mLocalStorage.RemoveItemAsync( "authToken" );
            await mLocalStorage.RemoveItemAsync( "refreshToken" );

            if( mAuthStateProvider is AuthStateProvider authProvider ) {
                authProvider.NotifyUserLogout();
            }

            mHttpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
