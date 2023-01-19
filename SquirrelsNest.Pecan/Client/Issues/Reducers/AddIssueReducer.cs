using System;
using Fluxor;
using SquirrelsNest.Pecan.Client.Issues.Actions;
using SquirrelsNest.Pecan.Client.Issues.Store;
using SquirrelsNest.Pecan.Shared.Entities;
using System.Collections.Generic;

namespace SquirrelsNest.Pecan.Client.Issues.Reducers {
    // ReSharper disable once UnusedType.Global
    public static class AddIssueReducer {
        [ReducerMethod( typeof( AddIssueSubmitAction ))]
        public static IssueState AddIssueSubmit( IssueState state ) =>
            new ( true, String.Empty, state.Issues, state.PageInformation );

        [ReducerMethod]
        public static IssueState AddIssueSuccess( IssueState state, AddIssueSuccess action ) {
            var issueList = new List<SnCompositeIssue> { action.Issue };

            issueList.AddRange( state.Issues );

            return new IssueState( false, String.Empty, issueList, state.PageInformation.IncreaseTotal );
        }

        [ReducerMethod]
        public static  IssueState AddIssueFailure( IssueState state, AddIssueFailure action ) =>
            new ( false, action.Message, state.Issues, state.PageInformation );
    }
}
