using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using MoreLinq;
using MvvmSupport.DialogService;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Logging;
using SquirrelsNest.Core.Extensions;
using SquirrelsNest.Core.Interfaces;
using SquirrelsNest.Desktop.Models;
using SquirrelsNest.Desktop.ViewModels.UiModels;
using SquirrelsNest.Desktop.Views;

namespace SquirrelsNest.Desktop.ViewModels {
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class IssueListViewModel : ObservableObject, IDisposable {
        private readonly CompositeDisposable    mSubscriptions;
        private readonly SynchronizationContext mContext;
        private readonly IProjectBuilder        mProjectBuilder;
        private readonly IProjectProvider       mProjectProvider;
        private readonly IIssueProvider         mIssueProvider;
        private readonly IIssueBuilder          mIssueBuilder;
        private readonly IModelState            mModelState;
        private readonly IDialogService         mDialogService;
        private readonly ILog                   mLog;
        private SnUser                          mCurrentUser;
        private Option<SnProject>               mCurrentProject;
        private bool                            mDisplayIssuesForAllUsers;
        private bool                            mDisplayFinalizedIssues;

        public  string                          ProjectName { get; private set; }
        public  ObservableCollection<UiIssue>   IssueList { get; }
        public  IRelayCommand<bool>             ViewDisplayed { get; }
        public  IRelayCommand                   CreateIssue { get; }

        // ReSharper disable UnusedAutoPropertyAccessor.Global
        public  IRelayCommand<UiIssue>          IssueCompleted { get; }
        public  IRelayCommand<UiIssue>          EditIssue {  get; }
        public  IRelayCommand<UiIssue>          DeleteIssue {  get; }
        // ReSharper restore UnusedAutoPropertyAccessor.Global

        public IssueListViewModel( IModelState modelState, IProjectProvider projectProvider, IIssueProvider issueProvider, IIssueBuilder issueBuilder,
                                   ILog log, SynchronizationContext context, IDialogService dialogService, IProjectBuilder projectBuilder ) {
            mIssueProvider = issueProvider;
            mProjectProvider = projectProvider;
            mModelState = modelState;
            mLog = log;
            mDialogService = dialogService;
            mProjectBuilder = projectBuilder;
            mIssueBuilder = issueBuilder;
            mContext = context;
            mCurrentUser = SnUser.Default;
            mDisplayIssuesForAllUsers = true;
            mDisplayFinalizedIssues = true;

            mSubscriptions = new CompositeDisposable();
            IssueList = new ObservableCollection<UiIssue>();
            ProjectName = String.Empty;
            ViewDisplayed = new RelayCommand<bool>( OnViewDisplayed );
            CreateIssue = new RelayCommand( OnCreateIssue );

            IssueCompleted = new RelayCommand<UiIssue>( OnIssueCompleted );
            EditIssue = new RelayCommand<UiIssue>( OnEditIssue );
            DeleteIssue = new RelayCommand<UiIssue>( OnDeleteIssue );
        }

        public bool ShowCompletedIssues {
            get => mDisplayFinalizedIssues;
            set {
                SetProperty( ref mDisplayFinalizedIssues, value );

                LoadIssueList();
            }
        }

        public bool ShowAssignedIssues {
            get => !mDisplayIssuesForAllUsers;
            set {
                SetProperty( ref mDisplayIssuesForAllUsers, !value );

                LoadIssueList();
            }
        }

        private void OnViewDisplayed( bool isLoading ) {
            if( isLoading ) {
                mSubscriptions.Add( mModelState.OnStateChange.ObserveOn( mContext ).Subscribe( OnModelStateChanged ));
                mSubscriptions.Add( mIssueProvider.OnEntitySourceChange.ObserveOn( mContext ).Subscribe( OnIssueListChanged ));
            }
            else {
                mSubscriptions.Clear();
            }
        }

        private void OnModelStateChanged( CurrentState state ) {
            mCurrentProject = state.Project;
            mCurrentUser = state.User;

            mCurrentProject.Do( project => {
                ProjectName = project.Name;

                OnPropertyChanged( nameof( ProjectName ));
            });

            LoadIssueList();
        }

        private void OnIssueListChanged( EntitySourceChange change ) {
            LoadIssueList();
        }

        private bool ShouldIssueBeDisplayed( UiIssue issue ) {
            var forUser = mDisplayIssuesForAllUsers || issue.AssignedUser.EntityId.Equals( mCurrentUser.EntityId );
            var isActive = mDisplayFinalizedIssues || !issue.IsFinalized;

            return forUser && isActive;
        }

        private void LoadIssueList() {
            IssueList.Clear();

            mCurrentProject
                .ToEither( new Error())
                .BindAsync( project => mIssueProvider.GetIssues( project )).Result
                .Map( list => from i in list select BuildIssue( i ))
                .Map( list => from i in list where ShouldIssueBeDisplayed( i ) select i )
                .Map( list => from i in list orderby i.IsFinalized, i.IssueNumber select i )
                .Match( list => list.ForEach( i => IssueList.Add( i )),
                        error => mLog.LogError( error ));
        }

        private UiIssue BuildIssue( SnIssue issue ) { 
            return new UiIssue( mIssueBuilder.BuildCompositeIssue( issue ));
        }

        private void OnIssueCompleted( UiIssue ? issue ) {
            if( issue != null ) {
                mCurrentProject.Do( snProject => {
                    var project = mProjectBuilder.BuildCompositeProject( snProject );
                    var newIssue = issue.Issue.ToggleCompletedState( project.WorkflowStates );

                    mIssueProvider
                        .UpdateIssue( newIssue ).Result
                        .IfLeft( error => mLog.LogError( error ));
                });
            }
        }

        private void OnCreateIssue() {
            if( mCurrentProject.IsSome ) {
                var project = mCurrentProject.AsEnumerable().First();
                var composite = mProjectBuilder.BuildCompositeProject( project );
                var parameters = new DialogParameters {{ EditIssueDialogViewModel.cProjectParameter, composite },     
                                                       { EditIssueDialogViewModel.cUserParameter, mCurrentUser }};

                mDialogService.ShowDialog( nameof( EditIssueDialog ), parameters, result => {
                    if( result.Result == ButtonResult.Ok ) {
                        var issue = result.Parameters.GetValue<SnIssue>( EditIssueDialogViewModel.cIssueParameter );

                        if( issue == null ) throw new ApplicationException( "Issue was not returned when editing issue" );

                        project = project.WithNextIssueNumber();

                        mIssueProvider
                            .AddIssue( issue ).Result
                            .Match( _ => {
                                    mProjectProvider
                                        .UpdateProject( project ).Result
                                        .Do( _ => mCurrentProject = project  )
                                        .IfLeft( error => mLog.LogError( error ));
                                },
                                error => mLog.LogError( error ));
                    }
                });
            }
        }

        private void OnEditIssue( UiIssue ?  uiIssue ) {
            if(( mCurrentProject.IsSome ) &&
               ( uiIssue != null )) {
                var project = mProjectBuilder.BuildCompositeProject( mCurrentProject.AsEnumerable().First());
                var parameters = new DialogParameters{{ EditIssueDialogViewModel.cProjectParameter, project },
                                                      { EditIssueDialogViewModel.cUserParameter, mCurrentUser },
                                                      { EditIssueDialogViewModel.cIssueParameter, uiIssue.Issue }};

                mDialogService.ShowDialog( nameof( EditIssueDialog ), parameters, result => {
                    if( result.Result == ButtonResult.Ok ) {
                        var issue = result.Parameters.GetValue<SnIssue>( EditIssueDialogViewModel.cIssueParameter );

                        if( issue == null ) throw new ApplicationException( "Issue was not returned when editing issue" );

                        mIssueProvider
                            .UpdateIssue( issue ).Result
                            .Match( _ => LoadIssueList(),
                                    error => mLog.LogError( error ));
                    }
                });
            }
        }

        private void OnDeleteIssue( UiIssue ? issue ) {
            if( issue != null ) {
                var parameters = new DialogParameters{{ ConfirmationDialogViewModel.cConfirmationText, $"Would you like to delete issue {issue.IssueNumber}?" }};

                mDialogService.ShowDialog( nameof( ConfirmationDialog ), parameters, result => {
                    if( result.Result == ButtonResult.Ok ) {
                        mIssueProvider.DeleteIssue( issue.Issue ).Result
                            .Match( _ => LoadIssueList(),
                                    error => mLog.LogError( error ));
                    }
                });
            }
        }

        public void Dispose() {
            mSubscriptions.Dispose();
        }
    }
}
