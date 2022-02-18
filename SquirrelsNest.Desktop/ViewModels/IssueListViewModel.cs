using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using LanguageExt.Common;
using MoreLinq;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Common.Logging;
using SquirrelsNest.Desktop.Models;

namespace SquirrelsNest.Desktop.ViewModels {
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class IssueListViewModel : IDisposable {
        // ReSharper disable once CollectionNeverQueried.Local
        private readonly CompositeDisposable    mSubscriptions;
        private readonly IIssueProvider         mIssueProvider;
        private readonly IModelState            mModelState;
        private readonly ILog                   mLog;

        public  ObservableCollection<SnIssue>   IssueList { get; }

        public IssueListViewModel( IModelState modelState, IIssueProvider issueProvider, ILog log, SynchronizationContext context ) {
            mIssueProvider = issueProvider;
            mModelState = modelState;
            mLog = log;

            mSubscriptions = new CompositeDisposable();
            IssueList = new ObservableCollection<SnIssue>();

            mSubscriptions.Add( modelState.OnStateChange.ObserveOn( context ).Subscribe( OnModelStateChanged ));
            mSubscriptions.Add( mIssueProvider.OnEntitySourceChange.ObserveOn( context ).Subscribe( OnIssueListChanged ));
        }

        private void OnModelStateChanged( CurrentState state ) {
            LoadIssueList( state );
        }

        private void OnIssueListChanged( EntitySourceChange change ) {
            LoadIssueList( mModelState.CurrentState );
        }

        private void LoadIssueList( CurrentState forState ) {
            IssueList.Clear();

            forState.Project.ToEither( new Error())
                .BindAsync( project => mIssueProvider.GetIssues( project )).Result
                .Match( list => list.ForEach( i => IssueList.Add( i )),
                        error => mLog.LogError( error ));
        }

        public void Dispose() {
            mSubscriptions.Clear();
        }
    }
}
