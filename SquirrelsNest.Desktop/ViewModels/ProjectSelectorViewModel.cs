using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Logging;
using SquirrelsNest.Core.CompositeBuilders;
using SquirrelsNest.Core.Extensions;
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
        private Option<SnUser>                          mCurrentUser;
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
            mCurrentUser = Option<SnUser>.None;

            ProjectList = new RangeCollection<CompositeProject>();
            mSubscriptions = new CompositeDisposable();

            ViewDisplayed = new RelayCommand<bool>( OnViewDisplayed );
        }

        private async void OnViewDisplayed( bool isLoading ) {
            if( isLoading ) {
                await LoadProjectList();

                mSubscriptions.Add( mModelState.OnStateChange.ObserveOn( mContext ).Subscribe( OnStateChanged ));
                mSubscriptions.Add( mProjectProvider.OnEntitySourceChange.ObserveOn( mContext ).SubscribeAsync( OnProjectsChanged, OnError ));
                mSubscriptions.Add( mProjectBuilder.OnProjectPartsChanged.ObserveOn( mContext ).SubscribeAsync( OnProjectsChanged, OnError ));
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

        private async Task<Unit> OnProjectsChanged( EntitySourceChange change ) {
            await LoadProjectList();

            return Unit.Default;
        }

        private void OnError( Exception ex ) {
            mLog.LogException( $"During EntitySourceChanged/ProjectPartsChanged in {nameof( ProjectSelectorViewModel )}", ex );
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

        private async Task<Either<Error, IEnumerable<CompositeProject>>> GetCompositeProjects( IEnumerable<SnProject> projects ) {
            var retValue = new List<CompositeProject>();

            foreach( var project in projects ) {
                var composite = await mProjectBuilder.BuildCompositeProject( project );

                composite.Match(
                    c => retValue.Add( c ), 
                    error => mLog.LogError( error ));
            }

            return retValue;
        }

        private async Task LoadProjectList() {
            var currentProject = mCurrentProject;
            var projects = await mProjectProvider.GetProjects();
            var composites = await projects.BindAsync( GetCompositeProjects );
            
            composites
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
