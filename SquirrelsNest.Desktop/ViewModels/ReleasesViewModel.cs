using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using LanguageExt;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using MoreLinq;
using MvvmSupport.DialogService;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Logging;
using SquirrelsNest.Core.Extensions;
using SquirrelsNest.Core.Interfaces;
using SquirrelsNest.Core.Models;
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
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        public  IRelayCommand<SnRelease>        EditRelease { get; }
        public  IRelayCommand<SnRelease>        DeleteRelease { get; }
        // ReSharper restore UnusedAutoPropertyAccessor.Global

        public ReleasesViewModel( IReleaseProvider releaseProvider, IModelState modelState, IDialogService dialogService, ILog log ) {
            mReleaseProvider = releaseProvider;
            mDialogService = dialogService;
            mLog = log;

            ReleaseList = new ObservableCollection<SnRelease>();
            CreateRelease = new RelayCommand( OnCreateRelease );
            EditRelease = new RelayCommand<SnRelease>( OnEditRelease );
            DeleteRelease = new RelayCommand<SnRelease>( OnDeleteRelease );

            mStateSubscription = modelState.OnStateChange.SubscribeAsync( OnStateChanged, OnError );
        }

        private async Task<Unit> OnStateChanged( CurrentState state ) {
            state.Project.Do( project => mCurrentProject = project );

            if( state.Project.IsSome ) {
                await LoadReleases();
            }

            return Unit.Default;
        }

        private void OnError( Exception ex ) {
            mLog.LogException( $"During ModelStateChanged in {nameof( ReleasesViewModel )}", ex );
        }

        private async Task LoadReleases() {
            if( mCurrentProject != null ) {
                ReleaseList.Clear();

                ( await mReleaseProvider.GetReleases( mCurrentProject ))
                    .Match( list => list.ForEach( p => ReleaseList.Add( p )),
                            error => mLog.LogError( error ));
            }
        }

        private void OnCreateRelease() {
            if( mCurrentProject != null ) {
                var parameters = new DialogParameters();

                mDialogService.ShowDialog( nameof( EditReleaseDialog ), parameters, async result => {
                    if( result.Result == ButtonResult.Ok ) {
                        var release = result.Parameters.GetValue<SnRelease>( EditReleaseDialogViewModel.cReleaseParameter );

                        if( release == null ) throw new ApplicationException( "SnRelease was not returned when editing issue" );

                        ( await mReleaseProvider.AddRelease( release.For( mCurrentProject )))
                            .IfLeft( error => mLog.LogError( error ));

                        await LoadReleases();
                    }
                });
            }
        }

        private void OnEditRelease( SnRelease ? editRelease ) {
            if(( mCurrentProject != null ) &&
               ( editRelease != null ) ) {
                var parameters = new DialogParameters{{ EditReleaseDialogViewModel.cReleaseParameter, editRelease }};

                mDialogService.ShowDialog( nameof( EditReleaseDialog ), parameters, async result => {
                    if( result.Result == ButtonResult.Ok ) {
                        var release = result.Parameters.GetValue<SnRelease>( EditReleaseDialogViewModel.cReleaseParameter );

                        if( release == null ) throw new ApplicationException( "SnRelease was not returned when editing issue" );

                        ( await mReleaseProvider.UpdateRelease( release.For( mCurrentProject )))
                            .IfLeft( error => mLog.LogError( error ));

                        await LoadReleases();
                    }
                });
            }
        }

        private void OnDeleteRelease( SnRelease ? release ) {
            if( release != null ) {
                var parameters = new DialogParameters {
                    { ConfirmationDialogViewModel.cConfirmationText, $"Would you like to delete the release named '{release.Name}'?" }
                };

                mDialogService.ShowDialog( nameof( ConfirmationDialog ), parameters, async result => {
                    if( result.Result == ButtonResult.Ok ) {
                        ( await mReleaseProvider.DeleteRelease( release ))
                            .IfLeft( error => mLog.LogError( error ));

                        await LoadReleases();
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
