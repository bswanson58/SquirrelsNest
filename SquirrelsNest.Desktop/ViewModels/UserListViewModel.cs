using System;
using System.Linq;
using System.Reactive.Disposables;
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
        private readonly CompositeDisposable    mSubscriptions;
        private Option<SnUser>                  mCurrentUser;
        private SnUser ?                        mSelectedUser;

        public  RangeCollection<SnUser>         UserList { get; }
        
        public  IRelayCommand                   CreateUser { get; }

        public UserListViewModel( IModelState modelState, IUserProvider userProvider, IDialogService dialogService, ILog log ) {
            mModelState = modelState;
            mUserProvider = userProvider;
            mDialogService = dialogService;
            mLog = log;
            mCurrentUser = Option<SnUser>.None;
            mSelectedUser = null;
            mSubscriptions = new CompositeDisposable();

            UserList = new RangeCollection<SnUser>();
            CreateUser = new RelayCommand( OnCreateUser );

            mSubscriptions.Add( mModelState.OnStateChange.SubscribeAsync( OnStateChanged, OnError ));
            mSubscriptions.Add( mUserProvider.OnEntitySourceChange.SubscribeAsync( OnUserListChanged, OnError ));
        }

        private async Task OnStateChanged( CurrentState state ) {
            if( state.User != mCurrentUser ) {
                mCurrentUser = state.User;

                await LoadUserList();
                SetSelectedUser();
            }
        }

        private async Task OnUserListChanged( EntitySourceChange _ ) {
            await LoadUserList();
        }

        private void OnError( Exception ex ) {
            mLog.LogException( $"During ModelStateChanged in {nameof( UserListViewModel )}", ex );
        }

        private void SetSelectedUser() {
            mCurrentUser.Do( user => mSelectedUser = UserList.FirstOrDefault( p => p.EntityId.Equals( user.EntityId )));
            if( mCurrentUser.IsNone ) {
                mSelectedUser = null;
            }

            OnPropertyChanged( nameof( SelectedUser ));
        }

        public SnUser ? SelectedUser {
            get => mSelectedUser;
            set {
                if(( mSelectedUser != value ) &&
                   ( value != null )) {
                    SetProperty( ref mSelectedUser, value );

                    if( mSelectedUser != null ) {
                        mModelState.SetUser( mSelectedUser );
                    }
                    else {
                        mModelState.ClearUser();
                    }
                }
            }
        }

        private async Task LoadUserList() {
            ( await mUserProvider.GetUsers())
                .Match( list => UserList.Reset( list ),
                        error => mLog.LogError( error ));
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
                        SelectedUser = UserList.FirstOrDefault( u => u.Email.Equals( editedUser.Email ));
                    }
                }
            });
        }

        public void Dispose() {
            mUserProvider.Dispose();
            mSubscriptions.Dispose();
        }
    }
}
