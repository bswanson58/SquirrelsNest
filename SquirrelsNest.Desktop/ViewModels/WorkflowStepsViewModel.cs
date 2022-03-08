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
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        // ReSharper disable MemberCanBePrivate.Global
        public  IRelayCommand<SnWorkflowState>  EditState { get; }
        public  IRelayCommand<SnWorkflowState>  DeleteState { get; }
        // ReSharper restore UnusedAutoPropertyAccessor.Global
        // ReSharper restore MemberCanBePrivate.Global

        public WorkflowStepsViewModel( IWorkflowStateProvider stateProvider, IModelState modelState, IDialogService dialogService, ILog log ) {
            mStateProvider = stateProvider;
            mDialogService = dialogService;
            mLog = log;

            StateList = new ObservableCollection<SnWorkflowState>();
            CreateState = new RelayCommand( OnCreateState );
            EditState = new RelayCommand<SnWorkflowState>( OnEditState );
            DeleteState = new RelayCommand<SnWorkflowState>( OnDeleteState );

            mStateSubscription = modelState.OnStateChange.SubscribeAsync( OnStateChanged, OnError );
        }

        private async Task<Unit> OnStateChanged( CurrentState state ) {
            state.Project.Do( project => {
                mCurrentProject = project;
            });

            await LoadStates();

            return Unit.Default;
        }

        private void OnError( Exception ex ) {
            mLog.LogException( $"During ModelStateChanged in {nameof( WorkflowStepsViewModel )}", ex );
        }

        private async Task LoadStates() {
            if( mCurrentProject != null ) {
                StateList.Clear();

                ( await mStateProvider.GetStates( mCurrentProject ))
                    .Match( list => list.ForEach( p => StateList.Add( p )),
                        error => mLog.LogError( error ));
            }
        }

        private void OnCreateState() {
            if( mCurrentProject != null ) {
                var parameters = new DialogParameters();

                mDialogService.ShowDialog( nameof( EditWorkflowStepDialog ), parameters, async result => {
                    if( result.Result == ButtonResult.Ok ) {
                        var state = result.Parameters.GetValue<SnWorkflowState>( EditWorkflowStepDialogViewModel.cStateParameter );

                        if( state == null ) throw new ApplicationException( "SnWorkflowState was not returned when editing issue" );

                        ( await mStateProvider.AddState( state.For( mCurrentProject )))
                            .IfLeft( error => mLog.LogError( error ));

                        await LoadStates();
                    }
                });
            }
        }

        private void OnEditState( SnWorkflowState ? editState ) {
            if(( mCurrentProject != null ) &&
               ( editState != null )) {
                var parameters = new DialogParameters {{ EditWorkflowStepDialogViewModel.cStateParameter, editState }};

                mDialogService.ShowDialog( nameof( EditWorkflowStepDialog ), parameters, async result => {
                    if( result.Result == ButtonResult.Ok ) {
                        var state = result.Parameters.GetValue<SnWorkflowState>( EditWorkflowStepDialogViewModel.cStateParameter );

                        if( state == null ) throw new ApplicationException( "SnWorkflowState was not returned when editing issue" );

                        ( await mStateProvider.UpdateState( state.For( mCurrentProject )))
                            .IfLeft( error => mLog.LogError( error ));
                        
                        await LoadStates();
                    }
                });
            }
        }

        private void OnDeleteState( SnWorkflowState ? state ) {
            if( state != null ) {
                var parameters = new DialogParameters {
                    { ConfirmationDialogViewModel.cConfirmationText, $"Would you like to delete the state named '{state.Name}'?" }
                };

                mDialogService.ShowDialog( nameof( ConfirmationDialog ), parameters, async result => {
                    if( result.Result == ButtonResult.Ok ) {
                        ( await mStateProvider.DeleteState( state ))
                            .IfLeft( error => mLog.LogError( error ));

                        await LoadStates();
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
