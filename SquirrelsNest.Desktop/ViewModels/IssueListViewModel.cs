using System;
using System.Collections.ObjectModel;
using LanguageExt.Common;
using MoreLinq;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Interfaces;
using SquirrelsNest.Desktop.Models;

namespace SquirrelsNest.Desktop.ViewModels {
    internal class IssueListViewModel : IDisposable {
        private readonly IIssueProvider     mIssueProvider;
        private readonly IModelState        mModelState;
        private IDisposable ?               mModelStateSubscription;

        public  ObservableCollection<SnIssue>   IssueList { get; }

        public IssueListViewModel( IModelState modelState, IIssueProvider issueProvider ) {
            mModelState = modelState;
            mIssueProvider = issueProvider;

            IssueList = new ObservableCollection<SnIssue>();

            mModelStateSubscription = mModelState.OnStateChange.Subscribe( OnModelStateChanged );
        }

        private void OnModelStateChanged( CurrentState state ) {
            LoadIssueList( state );
        }

        private void LoadIssueList( CurrentState forState ) {
            IssueList.Clear();

            forState.Project.ToEither( new Error())
                .Bind( project => mIssueProvider.GetIssues( project ))
                .Match( list => list.ForEach( i => IssueList.Add( i )),
                        error => { });
        }

        public void Dispose() {
            mModelStateSubscription?.Dispose();
            mModelStateSubscription = null;
        }
    }
}
