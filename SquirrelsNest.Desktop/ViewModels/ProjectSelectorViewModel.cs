using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using FluentValidation;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Logging;
using SquirrelsNest.Core.CompositeBuilders;
using SquirrelsNest.Core.Interfaces;
using SquirrelsNest.Desktop.Models;
using SquirrelsNest.Desktop.Platform;

namespace SquirrelsNest.Desktop.ViewModels {
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class ProjectSelectorViewModel : ObservableObject, IDisposable {
        private readonly SynchronizationContext         mContext;
        private readonly IModelState                    mModelState;
        private readonly IProjectProvider               mProjectProvider;
        private readonly IProjectBuilder                mProjectBuilder;
        private readonly IValidator<CompositeProject>   mValidator;
        private readonly ILog                           mLog;
        private readonly CompositeDisposable            mSubscriptions;
        private CompositeProject ?                      mCurrentProject;

        public  RangeCollection<CompositeProject>       ProjectList { get; }

        public  IRelayCommand<bool>                     ViewDisplayed { get; }
        
        public ProjectSelectorViewModel( IModelState modelState, IProjectProvider projectProvider, IProjectBuilder projectBuilder,
                                         IValidator<CompositeProject> validator, ILog log, SynchronizationContext context ) {
            mModelState = modelState;
            mProjectProvider = projectProvider;
            mProjectBuilder = projectBuilder;
            mValidator = validator;
            mContext = context;
            mLog = log;

            ProjectList = new RangeCollection<CompositeProject>();
            mSubscriptions = new CompositeDisposable();

            ViewDisplayed = new RelayCommand<bool>( OnViewDisplayed );

            LoadProjectList();
        }

        private void OnViewDisplayed( bool isLoading ) {
            if( isLoading ) {
                mSubscriptions.Add( mModelState.OnStateChange.ObserveOn( mContext ).Subscribe( OnStateChanged ));
                mSubscriptions.Add( mProjectProvider.OnEntitySourceChange.ObserveOn( mContext ).Subscribe( OnProjectsChanged ));
                mSubscriptions.Add( mProjectBuilder.OnProjectPartsChanged.ObserveOn( mContext ).Subscribe( OnProjectsChanged ));
            }
            else {
                mSubscriptions.Clear();
            }
        }

        private void OnStateChanged( CurrentState state ) {
            state.Project
                .Do( project => {
                    mCurrentProject = ProjectList.FirstOrDefault( p => p.Project.EntityId.Equals( project.EntityId ));

                    OnPropertyChanged( nameof( CurrentProject ));
                });
        }

        private void OnProjectsChanged( EntitySourceChange change ) {
            LoadProjectList();
        }

        public CompositeProject ?  CurrentProject {
            get => mCurrentProject;
            set {
                if( value != null ) {
                    SetProperty( ref mCurrentProject, value );

                    if( mCurrentProject != null ) {
                        mModelState.SetProject( mCurrentProject.Project );
                    }
                }
            }
        }

        private void LoadProjectList() {
            var currentProject = mCurrentProject;

            mProjectProvider
                .GetProjects().Result
                .Map( list => from project in list select mProjectBuilder.BuildCompositeProject( project ))
                .Map( list => from project in list where mValidator.Validate( project ).IsValid select project )
                .Match( list => ProjectList.Reset( list ),
                        error => mLog.LogError( error ));

            mCurrentProject = currentProject == null ? 
                ProjectList.FirstOrDefault() : 
                ProjectList.FirstOrDefault( p => p.Project.EntityId.Equals( currentProject.Project.EntityId ));
            OnPropertyChanged( nameof( CurrentProject ));
        }

        public void Dispose() {
            mProjectProvider.Dispose();
            mProjectBuilder.Dispose();
            mSubscriptions.Dispose();
        }
    }
}
