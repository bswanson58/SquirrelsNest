using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using MvvmSupport.DialogService;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Logging;
using SquirrelsNest.Core.Extensions;
using SquirrelsNest.Core.Interfaces;
using SquirrelsNest.Core.ProjectTemplates;
using SquirrelsNest.Core.Transfer.Import;
using SquirrelsNest.Desktop.Models;
using SquirrelsNest.Desktop.Platform;
using SquirrelsNest.Desktop.Views;

namespace SquirrelsNest.Desktop.ViewModels {
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class ProjectListViewModel : ObservableObject, IDisposable {
        private readonly SynchronizationContext     mContext;
        private readonly IModelState                mModelState;
        private readonly IProjectProvider           mProjectProvider;
        private readonly IProjectTemplateManager    mTemplateManager;
        private readonly IImportManager             mImportManager;
        private readonly IDialogService             mDialogService;
        private readonly ILog                       mLog;
        private readonly CompositeDisposable        mSubscriptions;
        private Option<SnUser>                      mCurrentUser;
        private SnProject ?                         mCurrentProject;

        public  RangeCollection<SnProject>          ProjectList { get; }
        
        public  IRelayCommand                       CreateProject { get; }
        public  IRelayCommand                       ImportProject { get; }
        public  IRelayCommand<bool>                 ViewDisplayed { get; }
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        public  IRelayCommand<SnProject>            EditProject { get; }
        public  IRelayCommand<SnProject>            DeleteProject { get; }
        // ReSharper enable UnusedAutoPropertyAccessor.Global

        public ProjectListViewModel( IModelState modelState, IProjectProvider projectProvider, IProjectTemplateManager templateManager,
                                     IImportManager importManager, IDialogService dialogService, ILog log, SynchronizationContext context ) {
            mModelState = modelState;
            mProjectProvider = projectProvider;
            mTemplateManager = templateManager;
            mImportManager = importManager;
            mDialogService = dialogService;
            mContext = context;
            mLog = log;
            mCurrentUser = Option<SnUser>.None;

            mSubscriptions = new CompositeDisposable();
            ProjectList = new RangeCollection<SnProject>();
            CreateProject = new RelayCommand( OnCreateProject );
            ImportProject = new RelayCommand( OnImportProject );
            ViewDisplayed = new RelayCommand<bool>( OnViewDisplayed );
            EditProject = new RelayCommand<SnProject>( OnEditProject );
            DeleteProject = new RelayCommand<SnProject>( OnDeleteProject );
        }

        private async void OnViewDisplayed( bool isLoaded ) {
            if( isLoaded ) {
                await LoadProjectList();

                mSubscriptions.Add( mModelState.OnStateChange.ObserveOn( mContext ).SubscribeAsync( OnStateChanged, OnError ));
            }
            else {
                mSubscriptions.Clear();
            }
        }

        private async Task OnStateChanged( CurrentState state ) {
            mCurrentUser = state.User;

            await LoadProjectList();

            state.Project
                .Do( project => {
                    mCurrentProject = ProjectList.FirstOrDefault( p => p.EntityId.Equals( project.EntityId ));

                    OnPropertyChanged( nameof( CurrentProject ));
                });
        }

        private void OnError( Exception ex ) {
            mLog.LogException( $"During EntitySourceChanged/ProjectPartsChanged in {nameof( ProjectSelectorViewModel )}", ex );
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

        private async Task LoadProjectList() {
            if( mCurrentUser.IsSome ) {
                var currentProject = mCurrentProject;
                var projectList = await mCurrentUser.MapAsync( user => mProjectProvider.GetProjects( user ));

                projectList
                    .Match( list => ProjectList.Reset( list ),
                        error => mLog.LogError( error ));

                mCurrentProject = currentProject != null ? 
                    ProjectList.FirstOrDefault( p => p.EntityId.Equals( currentProject.EntityId )) : 
                    ProjectList.FirstOrDefault();
                OnPropertyChanged( nameof( CurrentProject ));
            }
        }

        private void OnCreateProject() {
            var parameters = new DialogParameters();

            mCurrentUser.Do( user => {
                mDialogService.ShowDialog( nameof( CreateProjectDialog ), parameters, async result => {
                    if( result.Result == ButtonResult.Ok ) {
                        var editedProject = result.Parameters.GetValue<SnProject>( CreateProjectDialogViewModel.cProject );
                        var template = result.Parameters.GetValue<ProjectTemplate>( CreateProjectDialogViewModel.cTemplate );
                        Either<Error, SnProject> createdProject;

                        if( editedProject == null ) throw new ApplicationException( "Dialog did not return a project" );

                        if( template != null ) {
                            var projectParameters = new ProjectParameters {
                                ProjectName = editedProject.Name, 
                                ProjectDescription = editedProject.Description, 
                                ProjectPrefix = editedProject.IssuePrefix
                            };

                            createdProject = await mTemplateManager.CreateProject( template, projectParameters, user );
                        }
                        else {
                            createdProject = await mProjectProvider.AddProject( editedProject, user );
                        }

                        await LoadProjectList();

                        createdProject
                            .Do( p => mModelState.SetProject( p ))
                            .IfLeft( error => mLog.LogError( error ));

                    }
                });
            });
        }

        private void OnEditProject( SnProject ? project ) {
            if( project != null ) {
                var parameters = new DialogParameters{{ EditProjectDialogViewModel.cProject, project }};

                mDialogService.ShowDialog( nameof( EditProjectDialog ), parameters, async result => {
                    if( result.Result == ButtonResult.Ok ) {
                        var editedProject = result.Parameters.GetValue<SnProject>( EditProjectDialogViewModel.cProject );

                        if( editedProject != null ) {
                            ( await mProjectProvider.UpdateProject( editedProject ))
                                .IfLeft( e => mLog.LogError( e ));

                            await LoadProjectList();
                        }
                    }
                });
            }
        }

        private void OnDeleteProject( SnProject ? project ) {
            if( project != null ) {
                mCurrentUser.Do( user => {
                    var parameters = new DialogParameters {
                        { ConfirmationDialogViewModel.cConfirmationText, $"Would you like to delete the project named '{project.Name}'?" }
                    };

                    mDialogService.ShowDialog( nameof( ConfirmationDialog ), parameters, async result => {
                        if( result.Result == ButtonResult.Ok ) {
                            ( await mProjectProvider.DeleteProject( project, user ))
                                .IfLeft( error => mLog.LogError( error ));

                            await LoadProjectList();
                        }
                    });
                });
            }
        }

        private void OnImportProject() {
            mCurrentUser.Do( user => {
                var parameters = new DialogParameters{{ ImportProjectDialogViewModel.cUserParameter, mCurrentUser }};

                mDialogService.ShowDialog( nameof( ImportProjectDialog ), parameters, async result => {
                    if( result.Result == ButtonResult.Ok ) {
                        var importParameters = result.Parameters.GetValue<ImportParameters>( ImportProjectDialogViewModel.cImportParameters );

                        if( importParameters == null ) throw new ApplicationException( "Dialog did not return ImportParameters." );

                        ( await mImportManager.ImportProject( importParameters, user ))
                            .IfLeft( error => mLog.LogError( error ));

                        await LoadProjectList();
                    }
                });
            });
        }

        public void Dispose() {
            mProjectProvider.Dispose();
            mSubscriptions.Dispose();
        }
    }
}
