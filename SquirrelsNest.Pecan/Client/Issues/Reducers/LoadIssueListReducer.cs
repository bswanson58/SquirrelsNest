using System;
using Fluxor;
using SquirrelsNest.Pecan.Client.Issues.Actions;
using SquirrelsNest.Pecan.Client.Issues.Store;

namespace SquirrelsNest.Pecan.Client.Issues.Reducers {
    public static class LoadIssueListReducer {
        [ReducerMethod( typeof( LoadIssueListAction ))]
        public static IssueState LoadIssueList( IssueState state ) =>
            new ( true, String.Empty, state.Issues );

        [ReducerMethod]
        public static IssueState LoadIssueListSuccess( IssueState state, LoadIssueListSuccessAction action ) =>
            new ( false, String.Empty, action.Issues );

        [ReducerMethod]
        public static IssueState LoadIssueListFailure( IssueState state, LoadIssueListFailureAction action ) =>
            new ( false, action.Message, state.Issues );
    }
}
