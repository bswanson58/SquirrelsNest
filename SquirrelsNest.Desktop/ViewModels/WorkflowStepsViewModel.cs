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
    internal class WorkflowStepsViewModel : ObservableObject, IDisposable {
        private readonly IWorkflowStateProvider mStateProvider;
        private readonly IDialogService         mDialogService;
        private readonly IDisposable            mStateSubscription;
        private readonly ILog                   mLog;
        private SnProject ?                     mCurrentProject;

        public  ObservableCollection<SnWorkflowState>   StateList { get; }

        public  IRelayCommand                   CreateState { get; }

        public WorkflowStepsViewModel( IWorkflowStateProvider stateProvider, IModelState modelState, IDialogService dialogService, ILog log ) {
            mStateProvider = stateProvider;
            mDialogService = dialogService;
            mLog = log;

            StateList = new ObservableCollection<SnWorkflowState>();
            CreateState = new RelayCommand( OnCreateState );

            mStateSubscription = modelState.OnStateChange.Subscribe( OnStateChanged);
        }

        private void OnStateChanged( CurrentState state ) {
            state.Project.Do( project => {
                LoadStates( project );

                mCurrentProject = project;
            });
        }

        private void LoadStates( SnProject forProject ) {
            StateList.Clear();

            mStateProvider
                .GetStates( forProject ).Result
                    .Match( list => list.ForEach( p => StateList.Add( p )),
                            error => mLog.LogError( error ));
        }

        private void OnCreateState() {
            if( mCurrentProject != null ) {
                var parameters = new DialogParameters();

                mDialogService.ShowDialog( nameof( EditWorkflowStepDialog ), parameters, result => {
                    if( result.Result == ButtonResult.Ok ) {
                        var state = result.Parameters.GetValue<SnWorkflowState>( EditWorkflowStepDialogViewModel.cStateParameter );

                        if( state == null ) throw new ApplicationException( "SnWorkflowState was not returned when editing issue" );

                        mStateProvider
                            .AddState( state.For( mCurrentProject )).Result
                            .Match( _ => LoadStates( mCurrentProject ),
                                error => mLog.LogError( error ));
                    }
                });
            }
        }

        public void Dispose() {
            mStateProvider.Dispose();
            mStateSubscription.Dispose();
        }
    }
}
