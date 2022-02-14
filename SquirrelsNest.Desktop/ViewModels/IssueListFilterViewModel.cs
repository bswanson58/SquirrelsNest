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
    internal class IssueListFilterViewModel : ObservableObject {
        private readonly IProjectProvider       mProjectProvider;
        private readonly IIssueProvider         mIssueProvider;
        private readonly IDialogService         mDialogService;
        private readonly IModelState            mModelState;
        private readonly ILog                   mLog;
        private SnProject ?                     mCurrentProject;

        public  ObservableCollection<SnProject> ProjectList { get; }
        public  IRelayCommand                   CreateProject { get; }
        public  IRelayCommand                   CreateIssue { get; }

        public IssueListFilterViewModel( IProjectProvider projects, IModelState modelState, IIssueProvider issueProvider, IDialogService dialogService, ILog log ) {
            mProjectProvider = projects;
            mModelState = modelState;
            mDialogService = dialogService;
            mLog = log;
            mIssueProvider = issueProvider;

            ProjectList = new ObservableCollection<SnProject>();

            CreateProject = new RelayCommand( OnCreateProject );
            CreateIssue = new RelayCommand( OnCreateIssue );

            LoadProjects();
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

        private void LoadProjects() {
            var currentProject = CurrentProject;

            ProjectList.Clear();

            mProjectProvider
                .GetProjects().Result
                .Do( projectList => projectList.ForEach( project => ProjectList.Add( project )))
                .IfLeft( _ => { });

            if( currentProject != null ) {
                CurrentProject = ProjectList.FirstOrDefault( p => p.EntityId.Equals( currentProject.EntityId ));
            }

            CurrentProject ??= ProjectList.FirstOrDefault();
        }

        private void OnCreateProject() {
            var parameters = new DialogParameters();

            mDialogService.ShowDialog( nameof( EditProjectDialog ), parameters, result => {
                if( result.Result == ButtonResult.Ok ) {
                    var editedProject = result.Parameters.GetValue<SnProject>( EditProjectDialogViewModel.cProject );

                    if( editedProject != null ) {
                        mProjectProvider
                            .AddProject( editedProject ).Result
                            .Do( _ => LoadProjects())
                            .IfLeft( error => mLog.LogError( error ));

                        mModelState.SetProject( editedProject );
                    }
                }
            });
        }

        private void OnCreateIssue() {
            if( mCurrentProject != null ) {
                var parameters = new DialogParameters{{ EditIssueDialogViewModel.cProjectParameter, mCurrentProject }};

                mDialogService.ShowDialog( nameof( EditIssueDialog ), parameters, result => {
                    if( result.Result == ButtonResult.Ok ) {
                        var issue = result.Parameters.GetValue<SnIssue>( EditIssueDialogViewModel.cIssueParameter );
                        var project = result.Parameters.GetValue<SnProject>( EditIssueDialogViewModel.cProjectParameter );

                        if( issue == null ) throw new ApplicationException( "Issue was not returned when editing issue" );
                        if( project == null ) throw new ApplicationException( "Project was not returned when editing issue" );

                        mIssueProvider
                            .AddIssue( issue ).Result
                            .Match( _ => {
                                        mProjectProvider
                                            .UpdateProject( project.WithNextIssueNumber()).Result
                                                .IfLeft( error => mLog.LogError( error ));
                                    },
                                    error => mLog.LogError( error ));
                    }
                });
            }
        }
    }
}
