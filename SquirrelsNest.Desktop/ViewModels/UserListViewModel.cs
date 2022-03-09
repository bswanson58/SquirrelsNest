using System;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using MvvmSupport.DialogService;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Logging;
using SquirrelsNest.Core.Extensions;
using SquirrelsNest.Desktop.Models;
using SquirrelsNest.Desktop.Platform;
using SquirrelsNest.Desktop.Views;

namespace SquirrelsNest.Desktop.ViewModels {
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class UserListViewModel : ObservableObject, IDisposable {
        private readonly IModelState            mModelState;
        private readonly IUserProvider          mUserProvider;
        private readonly IDialogService         mDialogService;
        private readonly ILog                   mLog;
        private readonly IDisposable            mStateSubscription;
        private SnUser ?                        mCurrentUser;

        public  RangeCollection<SnUser>         UserList { get; }
        
        public  IRelayCommand                   CreateUser { get; }

        public UserListViewModel( IModelState modelState, IUserProvider userProvider, IDialogService dialogService, ILog log ) {
            mModelState = modelState;
            mUserProvider = userProvider;
            mDialogService = dialogService;
            mLog = log;

            UserList = new RangeCollection<SnUser>();
            CreateUser = new RelayCommand( OnCreateUser );

            mStateSubscription = mModelState.OnStateChange.SubscribeAsync( OnStateChanged, OnError );
        }

        private async Task<Unit> OnStateChanged( CurrentState state ) {
            await LoadUserList();

            mCurrentUser = UserList.FirstOrDefault( p => p.EntityId.Equals( state.User.EntityId ));

            OnPropertyChanged( nameof( CurrentUser ));

            return Unit.Default;
        }

        private void OnError( Exception ex ) {
            mLog.LogException( $"During ModelStateChanged in {nameof( UserListViewModel )}", ex );
        }

        public SnUser ? CurrentUser {
            get => mCurrentUser;
            set {
                SetProperty( ref mCurrentUser, value );

                if( mCurrentUser != null ) {
                    mModelState.SetUser( mCurrentUser );
                }
                else {
                    mModelState.ClearProject();
                }
            }
        }

        private async Task LoadUserList() {
            ( await mUserProvider.GetUsers())
                .Match( list => UserList.Reset( list ),
                        error => mLog.LogError( error ));

            mCurrentUser = UserList.FirstOrDefault();
            OnPropertyChanged( nameof( CurrentUser ));
        }

        private void OnCreateUser() {
            var parameters = new DialogParameters();

            mDialogService.ShowDialog( nameof( EditUserDialog ), parameters, async result => {
                if( result.Result == ButtonResult.Ok ) {
                    var editedUser = result.Parameters.GetValue<SnUser>( EditUserDialogViewModel.cUserParameter );

                    if( editedUser != null ) {
                        ( await mUserProvider.AddUser( editedUser ))
                            .IfLeft( error => mLog.LogError( error ));

                        await LoadUserList();
                    }
                }
            });
        }

        public void Dispose() {
            mUserProvider.Dispose();
            mStateSubscription.Dispose();
        }
    }
}
