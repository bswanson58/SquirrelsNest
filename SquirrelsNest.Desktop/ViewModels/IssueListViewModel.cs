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
using SquirrelsNest.Core.Interfaces;
using SquirrelsNest.Desktop.Models;
using SquirrelsNest.Desktop.ViewModels.UiModels;
using SquirrelsNest.Desktop.Views;

namespace SquirrelsNest.Desktop.ViewModels {
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class IssueListViewModel : IDisposable {
        // ReSharper disable once CollectionNeverQueried.Local
        private readonly CompositeDisposable    mSubscriptions;
        private readonly IProjectBuilder        mProjectBuilder;
        private readonly IIssueProvider         mIssueProvider;
        private readonly IIssueBuilder          mIssueBuilder;
        private readonly IModelState            mModelState;
        private readonly IDialogService         mDialogService;
        private readonly ILog                   mLog;
        private Option<SnProject>               mCurrentProject;

        public  ObservableCollection<UiIssue>   IssueList { get; }

        public IssueListViewModel( IModelState modelState, IIssueProvider issueProvider, IIssueBuilder issueBuilder,
                                   ILog log, SynchronizationContext context, IDialogService dialogService, IProjectBuilder projectBuilder ) {
            mIssueProvider = issueProvider;
            mModelState = modelState;
            mLog = log;
            mDialogService = dialogService;
            mProjectBuilder = projectBuilder;
            mIssueBuilder = issueBuilder;

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
                .Match( list => list.ForEach( i => IssueList.Add( i )),
                        error => mLog.LogError( error ));
        }

        private UiIssue BuildIssue( SnIssue issue ) { 
            return new UiIssue( mIssueBuilder.BuildCompositeIssue( issue ), OnEditIssue );
        }

        private void OnEditIssue( UiIssue uiIssue ) {
            if( mCurrentProject.IsSome ) {
                var project = mProjectBuilder.BuildCompositeProject( mCurrentProject.AsEnumerable().First());
                var parameters = new DialogParameters{{ EditIssueDialogViewModel.cProjectParameter, project }, 
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
