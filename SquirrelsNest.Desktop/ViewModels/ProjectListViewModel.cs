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
    internal class ProjectListViewModel : ObservableObject, IDisposable {
        private readonly IModelState        mModelState;
        private readonly IProjectProvider   mProjectProvider;
        private readonly IDialogService     mDialogService;
        private readonly ILog               mLog;
        private readonly IDisposable        mStateSubscription;
        private SnProject ?                 mCurrentProject;

        public  ObservableCollection<SnProject>    ProjectList { get; }
        
        public  IRelayCommand               CreateProject { get; }

        public ProjectListViewModel( IModelState modelState, IProjectProvider projectProvider, IDialogService dialogService, ILog log ) {
            mModelState = modelState;
            mProjectProvider = projectProvider;
            mDialogService = dialogService;
            mLog = log;

            ProjectList = new ObservableCollection<SnProject>();
            CreateProject = new RelayCommand( OnCreateProject );

            LoadProjectList();

            mStateSubscription = mModelState.OnStateChange.Subscribe( OnStateChanged );
        }

        private void OnStateChanged( CurrentState state ) {
            state.Project
                .Do( project => {
                    mCurrentProject = ProjectList.FirstOrDefault( p => p.EntityId.Equals( project.EntityId ));

                    OnPropertyChanged( nameof( CurrentProject ));
                });
        }

        public SnProject ?  CurrentProject {
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
            ProjectList.Clear();

            mProjectProvider
                .GetProjects().Result
                    .Match( list => list.ForEach( p => ProjectList.Add( p )),
                            error => mLog.LogError( error ));

            CurrentProject = ProjectList.FirstOrDefault();
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
            mStateSubscription.Dispose();
        }
    }
}
