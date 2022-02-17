using System;
using System.Collections.ObjectModel;
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
    internal class ReleasesViewModel : ObservableObject, IDisposable {
        private readonly IReleaseProvider   mReleaseProvider;
        private readonly IDialogService     mDialogService;
        private readonly IDisposable        mStateSubscription;
        private readonly ILog               mLog;
        private SnProject ?                 mCurrentProject;

        public  ObservableCollection<SnRelease> ReleaseList { get; }

        public  IRelayCommand                   CreateRelease { get; }

        public ReleasesViewModel( IReleaseProvider releaseProvider, IModelState modelState, IDialogService dialogService, ILog log ) {
            mReleaseProvider = releaseProvider;
            mDialogService = dialogService;
            mLog = log;

            ReleaseList = new ObservableCollection<SnRelease>();
            CreateRelease = new RelayCommand( OnCreateRelease );

            mStateSubscription = modelState.OnStateChange.Subscribe( OnStateChanged );
        }

        private void OnStateChanged( CurrentState state ) {
            state.Project.Do( project => {
                LoadReleases( project );

                mCurrentProject = project;
            });
        }

        private void LoadReleases( SnProject forProject ) {
            ReleaseList.Clear();

            mReleaseProvider
                .GetReleases( forProject ).Result
                .Match( list => list.ForEach( p => ReleaseList.Add( p )),
                    error => mLog.LogError( error ));
        }

        private void OnCreateRelease() {
            if( mCurrentProject != null ) {
                var parameters = new DialogParameters();

                mDialogService.ShowDialog( nameof( EditReleaseDialog ), parameters, result => {
                    if( result.Result == ButtonResult.Ok ) {
                        var release = result.Parameters.GetValue<SnRelease>( EditReleaseDialogViewModel.cReleaseParameter );

                        if( release == null ) throw new ApplicationException( "SnRelease was not returned when editing issue" );

                        mReleaseProvider
                            .AddRelease( release.For( mCurrentProject )).Result
                            .Match( _ => LoadReleases( mCurrentProject ),
                                error => mLog.LogError( error ));
                    }
                });
            }
        }

        public void Dispose() {
            mReleaseProvider.Dispose();
            mStateSubscription.Dispose();
        }
    }
}
