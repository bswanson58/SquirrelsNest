using System;
using System.Collections.Generic;
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
using SquirrelsNest.Desktop.Models;
using SquirrelsNest.Desktop.Platform;
using SquirrelsNest.Desktop.ViewModels.UiModels;
using SquirrelsNest.Desktop.Views;
using SquirrelsNest.Desktop.Views.ViewSupport;

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
        private readonly IRelayCommand          mFilterStateChanged;
        private SnUser                          mCurrentUser;
        private Option<SnProject>               mCurrentProject;
        private bool                            mDisplayIssuesForAllUsers;
        private bool                            mDisplayFinalizedIssues;

        public  string                          ProjectName { get; private set; }
        public  RangeCollection<UiIssue>        IssueList { get; }
        public  IRelayCommand<bool>             ViewDisplayed { get; }
        public  IRelayCommand                   CreateIssue { get; }

        public  IssueViewStyle ?                 DisplayStyle { get; private set; }
        public  IRelayCommand                   ToggleDisplayStyle { get; }

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
            DisplayStyle = IssueViewStyle.Everything;

            mSubscriptions = new CompositeDisposable();
            IssueList = new RangeCollection<UiIssue>();
            ProjectName = String.Empty;
            ViewDisplayed = new RelayCommand<bool>( OnViewDisplayed );
            CreateIssue = new AsyncRelayCommand( OnCreateIssue );
            ToggleDisplayStyle = new RelayCommand( OnToggleDisplayStyle );
            mFilterStateChanged = new AsyncRelayCommand( OnFilterStateChanged );

            IssueCompleted = new RelayCommand<UiIssue>( OnIssueCompleted );
            EditIssue = new AsyncRelayCommand<UiIssue>( OnEditIssue );
            DeleteIssue = new RelayCommand<UiIssue>( OnDeleteIssue );
        }

        public bool ShowCompletedIssues {
            get => mDisplayFinalizedIssues;
            set {
                SetProperty( ref mDisplayFinalizedIssues, value );

                mFilterStateChanged.Execute( Unit.Default );
            }
        }

        public bool ShowAssignedIssues {
            get => !mDisplayIssuesForAllUsers;
            set {
                SetProperty( ref mDisplayIssuesForAllUsers, !value );

                mFilterStateChanged.Execute( Unit.Default );
            }
        }

        private Task OnFilterStateChanged() {
            return LoadIssueList();
        }

        private void OnToggleDisplayStyle() {
            DisplayStyle = DisplayStyle switch {
                IssueViewStyle.Everything => IssueViewStyle.TitleEntities,
                IssueViewStyle.TitleEntities => IssueViewStyle.TitleDescription,
                IssueViewStyle.TitleDescription => IssueViewStyle.TitleOnly,
                IssueViewStyle.TitleOnly => IssueViewStyle.Everything,
                _ => IssueViewStyle.Everything
            };

            IssueList.AddRange( Array.Empty<UiIssue>());
        }

        private void OnViewDisplayed( bool isLoading ) {
            if( isLoading ) {
                mSubscriptions.Add( mModelState.OnStateChange.ObserveOn( mContext ).SubscribeAsync( OnModelStateChanged, OnError ));
                mSubscriptions.Add( mIssueProvider.OnEntitySourceChange.ObserveOn( mContext ).SubscribeAsync( OnIssueListChanged, OnError ));
            }
            else {
                mSubscriptions.Clear();
            }
        }

        private async Task OnModelStateChanged( CurrentState state ) {
            mCurrentProject = state.Project;
            mCurrentUser = state.User;

            mCurrentProject.Do( project => {
                ProjectName = project.Name;

                OnPropertyChanged( nameof( ProjectName ));
            });

            await LoadIssueList();
        }

        private async Task OnIssueListChanged( EntitySourceChange _ ) {
            await LoadIssueList();
        }

        private void OnError( Exception ex ) {
            mLog.LogException( $"During ModelStateChanged/IssueListChanged in {nameof( IssueListViewModel )}", ex );
        }

        private bool ShouldIssueBeDisplayed( UiIssue issue ) {
            var forUser = mDisplayIssuesForAllUsers || issue.AssignedUser.EntityId.Equals( mCurrentUser.EntityId );
            var isActive = mDisplayFinalizedIssues || !issue.IsFinalized;

            return forUser && isActive;
        }

        private async Task<Either<Error, IEnumerable<UiIssue>>> CreateUiIssues( IEnumerable<SnIssue> issues ) {
            var retValue = new List<UiIssue>();

            foreach( var issue in issues ) {
                var composite = await mIssueBuilder.BuildCompositeIssue( issue );

                composite.Match(
                    c => retValue.Add( new UiIssue( c )), 
                    error => mLog.LogError( error ));
            }

            return retValue;
        }

        private async Task LoadIssueList() {
            ( await mCurrentProject.
                ToEither( new Error())
                .BindAsync( project => mIssueProvider.GetIssues( project ))
                .BindAsync( CreateUiIssues ))
                .Map( list => from i in list where ShouldIssueBeDisplayed( i ) select i )
                .Map( list => from i in list orderby i.IsFinalized, i.IssueNumber select i )
                .Match( list => IssueList.Reset( list ),
                        error => mLog.LogError( error ));
        }

        private async void OnIssueCompleted( UiIssue ? issue ) {
            if( issue != null ) {
                var composite = await mCurrentProject.MapAsync( async project => await mProjectBuilder.BuildCompositeProject( project ));
                var updatedIssue = composite.Map( c => issue.Issue.ToggleCompletedState( c.WorkflowStates ));
                var e = await updatedIssue.BindAsync( async newIssue => await mIssueProvider.UpdateIssue( newIssue ));

                e.IfLeft( er => mLog.LogError( er ));
            }
        }

        private async Task OnCreateIssue() {
            if( mCurrentProject.IsSome ) {
                var project = mCurrentProject.AsEnumerable().First();
                var composite = await mProjectBuilder.BuildCompositeProject( project );

                composite.IfLeft( error => mLog.LogError( error ));
                composite.IfRight( compositeProject => {
                    var parameters = new DialogParameters {{ EditIssueDialogViewModel.cProjectParameter, compositeProject },
                                                           { EditIssueDialogViewModel.cUserParameter, mCurrentUser }};

                    mDialogService.ShowDialog( nameof( EditIssueDialog ), parameters, async result => {
                        if( result.Result == ButtonResult.Ok ) {
                            var issue = result.Parameters.GetValue<SnIssue>( EditIssueDialogViewModel.cIssueParameter );

                            if( issue == null ) throw new ApplicationException( "Issue was not returned when editing issue" );

                            project = project.WithNextIssueNumber();

                            ( await ( await mIssueProvider.AddIssue( issue ))
                                    .BindAsync( _ => mProjectProvider.UpdateProject( project )))
                                .Do( _ => mCurrentProject = project )
                                .IfLeft( error => mLog.LogError( error ));
                        }
                    });
                }); 
            }
        }

        private async Task OnEditIssue( UiIssue ?  uiIssue ) {
            if(( mCurrentProject.IsSome ) &&
               ( uiIssue != null )) {
                var project = mCurrentProject.AsEnumerable().First();
                var composite = await mProjectBuilder.BuildCompositeProject( project );

                composite.IfLeft( error => mLog.LogError( error ));
                composite.IfRight( compositeProject => {
                    var parameters = new DialogParameters{{ EditIssueDialogViewModel.cProjectParameter, compositeProject },
                                                          { EditIssueDialogViewModel.cUserParameter, mCurrentUser },
                                                          { EditIssueDialogViewModel.cIssueParameter, uiIssue.Issue }};

                    mDialogService.ShowDialog( nameof( EditIssueDialog ), parameters, async result => {
                        if( result.Result == ButtonResult.Ok ) {
                            var issue = result.Parameters.GetValue<SnIssue>( EditIssueDialogViewModel.cIssueParameter );

                            if( issue == null ) throw new ApplicationException( "Issue was not returned when editing issue" );

                            ( await mIssueProvider.UpdateIssue( issue ))
                                .IfLeft( error => mLog.LogError( error ));

                            await LoadIssueList();
                        }
                    });
                });
            }
        }

        private void OnDeleteIssue( UiIssue ? issue ) {
            if( issue != null ) {
                var parameters = new DialogParameters{{ ConfirmationDialogViewModel.cConfirmationText, $"Would you like to delete issue {issue.IssueNumber}?" }};

                mDialogService.ShowDialog( nameof( ConfirmationDialog ), parameters, async result => {
                    if( result.Result == ButtonResult.Ok ) {
                        ( await mIssueProvider.DeleteIssue( issue.Issue ))
                            .IfLeft( error => mLog.LogError( error ));

                        await LoadIssueList();
                    }
                });
            }
        }

        public void Dispose() {
            mSubscriptions.Dispose();
        }
    }
}
