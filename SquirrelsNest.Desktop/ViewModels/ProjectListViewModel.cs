using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
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
    internal class ProjectListViewModel : ObservableObject, IDisposable {
        private readonly SynchronizationContext     mContext;
        private readonly IModelState                mModelState;
        private readonly IProjectProvider           mProjectProvider;
        private readonly IDialogService             mDialogService;
        private readonly ILog                       mLog;
        private readonly CompositeDisposable        mSubscriptions;
        private SnProject ?                         mCurrentProject;

        public  ObservableCollection<SnProject>     ProjectList { get; }
        
        public  IRelayCommand                       CreateProject { get; }
        public  IRelayCommand<bool>                 ViewDisplayed { get; }

        public ProjectListViewModel( IModelState modelState, IProjectProvider projectProvider, IDialogService dialogService, ILog log, SynchronizationContext context ) {
            mModelState = modelState;
            mProjectProvider = projectProvider;
            mDialogService = dialogService;
            mContext = context;
            mLog = log;

            mSubscriptions = new CompositeDisposable();
            ProjectList = new ObservableCollection<SnProject>();
            CreateProject = new RelayCommand( OnCreateProject );
            ViewDisplayed = new RelayCommand<bool>( OnViewDisplayed );

            LoadProjectList();
        }

        private void OnViewDisplayed( bool isLoaded ) {
            if( isLoaded ) {
                mSubscriptions.Add( mModelState.OnStateChange.ObserveOn( mContext ).Subscribe( OnStateChanged ));
            }
            else {
                mSubscriptions.Dispose();
            }
        }

        private void OnStateChanged( CurrentState state ) {
            state.Project
                .Do( project => {
                    if(!project.EntityId.Equals( mCurrentProject?.EntityId )) {
                        mCurrentProject = ProjectList.FirstOrDefault( p => p.EntityId.Equals( project.EntityId ));

                        OnPropertyChanged( nameof( CurrentProject ));
                    }
                });
        }

        public SnProject ? CurrentProject {
            get => mCurrentProject;
            set {
                SetProperty( ref mCurrentProject, value );

                if( mCurrentProject != null ) {
                    mModelState.SetProject( mCurrentProject );
                }
                else {
                    mModelState.ClearProject();
                }
            }
        }

        private void LoadProjectList() {
            var currentProject = mCurrentProject;

            ProjectList.Clear();

            mProjectProvider
                .GetProjects().Result
                    .Match( list => list.ForEach( p => ProjectList.Add( p )),
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
                            .Do( _ => mModelState.SetProject( editedProject ))
                            .IfLeft( error => mLog.LogError( error ));

                        mModelState.SetProject( editedProject );
                    }
                }
            });
        }

        public void Dispose() {
            mProjectProvider.Dispose();
            mSubscriptions.Dispose();
        }
    }
}
