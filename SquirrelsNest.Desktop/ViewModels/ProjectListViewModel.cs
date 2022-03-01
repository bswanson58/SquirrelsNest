using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using MvvmSupport.DialogService;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Logging;
using SquirrelsNest.Desktop.Models;
using SquirrelsNest.Desktop.Platform;
using SquirrelsNest.Desktop.Views;

namespace SquirrelsNest.Desktop.ViewModels {
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class ProjectListViewModel : ObservableObject, IDisposable {
        private readonly SynchronizationContext     mContext;
        private readonly IModelState                mModelState;
        private readonly IProjectProvider           mProjectProvider;
        private readonly IDialogService             mDialogService;
        private readonly ILog                       mLog;
        private readonly CompositeDisposable        mSubscriptions;
        private SnProject ?                         mCurrentProject;

        public  RangeCollection<SnProject>          ProjectList { get; }
        
        public  IRelayCommand                       CreateProject { get; }
        public  IRelayCommand<bool>                 ViewDisplayed { get; }
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public  IRelayCommand<SnProject>            EditProject { get; }

        public ProjectListViewModel( IModelState modelState, IProjectProvider projectProvider, IDialogService dialogService, ILog log, SynchronizationContext context ) {
            mModelState = modelState;
            mProjectProvider = projectProvider;
            mDialogService = dialogService;
            mContext = context;
            mLog = log;

            mSubscriptions = new CompositeDisposable();
            ProjectList = new RangeCollection<SnProject>();
            CreateProject = new RelayCommand( OnCreateProject );
            ViewDisplayed = new RelayCommand<bool>( OnViewDisplayed );
            EditProject = new RelayCommand<SnProject>( OnEditProject );

            LoadProjectList();
        }

        private void OnViewDisplayed( bool isLoaded ) {
            if( isLoaded ) {
                mSubscriptions.Add( mModelState.OnStateChange.ObserveOn( mContext ).Subscribe( OnStateChanged ));
            }
            else {
                mSubscriptions.Clear();
            }
        }

        private void OnStateChanged( CurrentState state ) {
            state.Project
                .Do( project => {
                    mCurrentProject = ProjectList.FirstOrDefault( p => p.EntityId.Equals( project.EntityId ));

                    OnPropertyChanged( nameof( CurrentProject ));
                });
        }

        public SnProject ? CurrentProject {
            get => mCurrentProject;
            set {
                if( value != null ) {
                    SetProperty( ref mCurrentProject, value );

                    if( mCurrentProject != null ) {
                        mModelState.SetProject( mCurrentProject );
                    }
                }
            }
        }

        private void LoadProjectList() {
            var currentProject = mCurrentProject;

            mProjectProvider
                .GetProjects().Result
                .Match( list => ProjectList.Reset( list ),
                            error => mLog.LogError( error ));

            mCurrentProject = currentProject != null ? 
                ProjectList.FirstOrDefault( p => p.EntityId.Equals( currentProject.EntityId )) : 
                ProjectList.FirstOrDefault();
            OnPropertyChanged( nameof( CurrentProject ));
        }

        private void OnCreateProject() {
            var parameters = new DialogParameters();

            mDialogService.ShowDialog( nameof( EditProjectDialog ), parameters, result => {
                if( result.Result == ButtonResult.Ok ) {
                    var editedProject = result.Parameters.GetValue<SnProject>( EditProjectDialogViewModel.cProject );

                    if( editedProject != null ) {
                        mProjectProvider
                            .AddProject( editedProject ).Result
                            .Do( _ => LoadProjectList())
                            .Do( project => mModelState.SetProject( project ))
                            .IfLeft( error => mLog.LogError( error ));
                    }
                }
            });
        }

        private void OnEditProject( SnProject ? project ) {
            if( project != null ) {
                var parameters = new DialogParameters{{ EditProjectDialogViewModel.cProject, project }};

                mDialogService.ShowDialog( nameof( EditProjectDialog ), parameters, result => {
                    if( result.Result == ButtonResult.Ok ) {
                        var editedProject = result.Parameters.GetValue<SnProject>( EditProjectDialogViewModel.cProject );

                        if( editedProject != null ) {
                            mProjectProvider
                                .UpdateProject( editedProject ).Result
                                .Do( _ => LoadProjectList())
//                                .Do( project => mModelState.SetProject( project ))
                                .IfLeft( error => mLog.LogError( error ));
                        }
                    }
                });
            }
        }

        public void Dispose() {
            mProjectProvider.Dispose();
            mSubscriptions.Dispose();
        }
    }
}
