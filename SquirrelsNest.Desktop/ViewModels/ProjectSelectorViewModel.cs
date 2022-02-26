using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using FluentValidation;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using MoreLinq;
using MvvmSupport.DialogService;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Logging;
using SquirrelsNest.Core.CompositeBuilders;
using SquirrelsNest.Core.Interfaces;
using SquirrelsNest.Desktop.Models;
using SquirrelsNest.Desktop.Views;

namespace SquirrelsNest.Desktop.ViewModels {
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class ProjectSelectorViewModel : ObservableObject, IDisposable {
        private readonly IModelState                    mModelState;
        private readonly IProjectProvider               mProjectProvider;
        private readonly IProjectBuilder                mProjectBuilder;
        private readonly IDialogService                 mDialogService;
        private readonly IValidator<CompositeProject>   mValidator;
        private readonly ILog                           mLog;
        private readonly CompositeDisposable            mSubscriptions;
        private CompositeProject ?                      mCurrentProject;

        public  ObservableCollection<CompositeProject>  ProjectList { get; }
        
        public  IRelayCommand                           CreateProject { get; }

        public ProjectSelectorViewModel( IModelState modelState, IProjectProvider projectProvider, IProjectBuilder projectBuilder,
                                         IValidator<CompositeProject> validator, IDialogService dialogService, ILog log, SynchronizationContext context ) {
            mModelState = modelState;
            mProjectProvider = projectProvider;
            mProjectBuilder = projectBuilder;
            mDialogService = dialogService;
            mValidator = validator;
            mLog = log;

            ProjectList = new ObservableCollection<CompositeProject>();
            CreateProject = new RelayCommand( OnCreateProject );
            mSubscriptions = new CompositeDisposable();

            LoadProjectList();

//            mStateSubscription = mModelState.OnStateChange.Subscribe( OnStateChanged );
            mSubscriptions.Add( mProjectProvider.OnEntitySourceChange.ObserveOn( context ).Subscribe( OnProjectsChanged ));
            mSubscriptions.Add( mProjectBuilder.OnProjectPartsChanged.ObserveOn( context ).Subscribe( OnProjectsChanged ));
        }

        private void OnProjectsChanged( EntitySourceChange change ) {
            LoadProjectList();
        }

        private void OnStateChanged( CurrentState state ) {
            state.Project
                .Do( project => {
                    mCurrentProject = ProjectList.FirstOrDefault( p => p.Project.EntityId.Equals( project.EntityId ));

                    OnPropertyChanged( nameof( CurrentProject ));
                });
        }

        public CompositeProject ?  CurrentProject {
            get => mCurrentProject;
            set {
                SetProperty( ref mCurrentProject, value );

                if( mCurrentProject != null ) {
                    mModelState.SetProject( mCurrentProject.Project );
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
                .Map( list => from project in list select mProjectBuilder.BuildCompositeProject( project ))
                .Map( list => from project in list where mValidator.Validate( project ).IsValid select project )
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
            mProjectBuilder.Dispose();
            mSubscriptions.Dispose();
        }
    }
}
