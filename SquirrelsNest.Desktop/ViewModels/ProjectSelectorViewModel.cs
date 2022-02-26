using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using FluentValidation;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using MoreLinq;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Logging;
using SquirrelsNest.Core.CompositeBuilders;
using SquirrelsNest.Core.Interfaces;
using SquirrelsNest.Desktop.Models;

namespace SquirrelsNest.Desktop.ViewModels {
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class ProjectSelectorViewModel : ObservableObject, IDisposable {
        private readonly IModelState                    mModelState;
        private readonly IProjectProvider               mProjectProvider;
        private readonly IProjectBuilder                mProjectBuilder;
        private readonly IValidator<CompositeProject>   mValidator;
        private readonly ILog                           mLog;
        private readonly CompositeDisposable            mSubscriptions;
        private CompositeProject ?                      mCurrentProject;

        public  ObservableCollection<CompositeProject>  ProjectList { get; }
        
        public ProjectSelectorViewModel( IModelState modelState, IProjectProvider projectProvider, IProjectBuilder projectBuilder,
                                         IValidator<CompositeProject> validator, ILog log, SynchronizationContext context ) {
            mModelState = modelState;
            mProjectProvider = projectProvider;
            mProjectBuilder = projectBuilder;
            mValidator = validator;
            mLog = log;

            ProjectList = new ObservableCollection<CompositeProject>();
            mSubscriptions = new CompositeDisposable();

            LoadProjectList();

            mSubscriptions.Add( mProjectProvider.OnEntitySourceChange.ObserveOn( context ).Subscribe( OnProjectsChanged ));
            mSubscriptions.Add( mProjectBuilder.OnProjectPartsChanged.ObserveOn( context ).Subscribe( OnProjectsChanged ));
        }

        private void OnProjectsChanged( EntitySourceChange change ) {
            LoadProjectList();
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

        public void Dispose() {
            mProjectProvider.Dispose();
            mProjectBuilder.Dispose();
            mSubscriptions.Dispose();
        }
    }
}
