using System;
using System.Collections.Generic;
using System.Linq;
using Fluxor;
using SquirrelsNest.Pecan.Client.Issues.Actions;
using SquirrelsNest.Pecan.Client.Issues.Store;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.Issues.Reducers {
    // ReSharper disable once UnusedType.Global
    public static class LoadIssueListReducer {
        [ReducerMethod( typeof( LoadIssueListAction ))]
        public static IssueState LoadIssueList( IssueState state ) =>
            new ( true, String.Empty, state.Issues, state.PageInformation, state.CurrentProjectId, state.CurrentDisplayPage );

        [ReducerMethod]
        public static IssueState LoadIssueListSuccess( IssueState state, LoadIssueListSuccessAction action ) {
            var issues = new List<SnCompositeIssue>( state.Issues );

            issues.AddRange( action.Issues.Where( i => !state.Issues.Any( ie => ie.EntityId.Equals( i.EntityId ))));

            return new ( false, String.Empty, issues, action.PageInformation, state.CurrentProjectId, state.CurrentDisplayPage );
        }

        [ReducerMethod]
        public static IssueState LoadIssueListFailure( IssueState state, LoadIssueListFailureAction action ) =>
            new ( false, action.Message, state.Issues, state.PageInformation, state.CurrentProjectId, state.CurrentDisplayPage );
    }
}
