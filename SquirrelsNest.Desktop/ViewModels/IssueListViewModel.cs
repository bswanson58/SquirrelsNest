using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using LanguageExt;
using LanguageExt.Common;
using MoreLinq;
using MvvmSupport.DialogService;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Logging;
using SquirrelsNest.Desktop.Models;
using SquirrelsNest.Desktop.ViewModels.UiModels;
using SquirrelsNest.Desktop.Views;

namespace SquirrelsNest.Desktop.ViewModels {
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class IssueListViewModel : IDisposable {
        // ReSharper disable once CollectionNeverQueried.Local
        private readonly CompositeDisposable    mSubscriptions;
        private readonly IIssueProvider         mIssueProvider;
        private readonly IIssueTypeProvider     mIssueTypeProvider;
        private readonly IModelState            mModelState;
        private readonly IDialogService         mDialogService;
        private readonly ILog                   mLog;
        private Option<SnProject>               mCurrentProject;

        public  ObservableCollection<UiIssue>   IssueList { get; }

        public IssueListViewModel( IModelState modelState, IIssueProvider issueProvider, IIssueTypeProvider issueTypeProvider,
                                   ILog log, SynchronizationContext context, IDialogService dialogService ) {
            mIssueProvider = issueProvider;
            mModelState = modelState;
            mLog = log;
            mDialogService = dialogService;
            mIssueTypeProvider = issueTypeProvider;

            mSubscriptions = new CompositeDisposable();
            IssueList = new ObservableCollection<UiIssue>();

            mSubscriptions.Add( modelState.OnStateChange.ObserveOn( context ).Subscribe( OnModelStateChanged ));
            mSubscriptions.Add( mIssueProvider.OnEntitySourceChange.ObserveOn( context ).Subscribe( OnIssueListChanged ));
        }

        private void OnModelStateChanged( CurrentState state ) {
            mCurrentProject = state.Project;

            LoadIssueList( state.Project );
        }

        private void OnIssueListChanged( EntitySourceChange change ) {
            LoadIssueList( mModelState.CurrentState.Project );
        }

        private void LoadIssueList( Option<SnProject> forProject ) {
            IssueList.Clear();

            forProject
                .ToEither( new Error())
                .BindAsync( project => mIssueProvider.GetIssues( project )).Result
                .Map( list => from i in list select BuildIssue( i ))
                .Match( list => list.ForEach( oi => oi.Do( i => IssueList.Add( i ))),
                        error => mLog.LogError( error ));
        }

        private SnIssueType GetIssueType( SnIssue issue ) {
            return mIssueTypeProvider
                .GetIssue( issue.IssueTypeId ).Result
                .IfLeft( SnIssueType.Default );
        }

        private Option<UiIssue> BuildIssue( SnIssue issue ) {
            return mCurrentProject
                .Map( project => ( Project: project, IssueType: GetIssueType( issue )))
                .Map( t => new UiIssue( t.Project, issue, t.IssueType, OnEditIssue ));
        }

        private void OnEditIssue( UiIssue uiIssue ) {
            if( mCurrentProject.IsSome ) {
                var parameters = new DialogParameters{{ EditIssueDialogViewModel.cProjectParameter, mCurrentProject.AsEnumerable().First() }, 
                                                      { EditIssueDialogViewModel.cIssueParameter, uiIssue.Issue }};

                mDialogService.ShowDialog( nameof( EditIssueDialog ), parameters, result => {
                    if( result.Result == ButtonResult.Ok ) {
                        var issue = result.Parameters.GetValue<SnIssue>( EditIssueDialogViewModel.cIssueParameter );

                        if( issue == null ) throw new ApplicationException( "Issue was not returned when editing issue" );

                        mIssueProvider
                            .UpdateIssue( issue ).Result
                            .Match( _ => LoadIssueList( mCurrentProject ),
                                    error => mLog.LogError( error ));
                    }
                });
            }
        }

        public void Dispose() {
            mSubscriptions.Clear();
        }
    }
}
