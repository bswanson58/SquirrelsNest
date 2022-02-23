using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using MoreLinq;
using MvvmSupport.DialogService;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Logging;
using SquirrelsNest.Desktop.Models;
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

        public  ObservableCollection<SnUser>    UserList { get; }
        
        public  IRelayCommand                   CreateUser { get; }

        public UserListViewModel( IModelState modelState, IUserProvider userProvider, IDialogService dialogService, ILog log ) {
            mModelState = modelState;
            mUserProvider = userProvider;
            mDialogService = dialogService;
            mLog = log;

            UserList = new ObservableCollection<SnUser>();
            CreateUser = new RelayCommand( OnCreateUser );

            LoadUserList();

            mStateSubscription = mModelState.OnStateChange.Subscribe( OnStateChanged );
        }

        private void OnStateChanged( CurrentState state ) {
            state.Project
                .Do( project => {
                    mCurrentUser = UserList.FirstOrDefault( p => p.EntityId.Equals( project.EntityId ));

                    OnPropertyChanged( nameof( CurrentProject ));
                });
        }

        public SnUser ?  CurrentProject {
            get => mCurrentUser;
            set {
                SetProperty( ref mCurrentUser, value );

                if( mCurrentUser != null ) {
//                    mModelState.SetProject( mCurrentUser );
                }
                else {
                    mModelState.ClearProject();
                }
            }
        }

        private void LoadUserList() {
            UserList.Clear();

            mUserProvider
                .GetUsers().Result
                    .Match( list => list.ForEach( p => UserList.Add( p )),
                            error => mLog.LogError( error ));

            CurrentProject = UserList.FirstOrDefault();
        }

        private void OnCreateUser() {
            var parameters = new DialogParameters();

            mDialogService.ShowDialog( nameof( EditUserDialog ), parameters, result => {
                if( result.Result == ButtonResult.Ok ) {
                    var editedUser = result.Parameters.GetValue<SnUser>( EditUserDialogViewModel.cUserParameter );

                    if( editedUser != null ) {
                        mUserProvider
                            .AddUser( editedUser ).Result
                            .Do( _ => LoadUserList())
//                            .Do( _ => mModelState.SetProject( editedUser ))
                            .IfLeft( error => mLog.LogError( error ));

//                        mModelState.SetProject( editedUser );
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
