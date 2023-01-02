using Fluxor;
using SquirrelsNest.Pecan.Client.Issues.Actions;
using SquirrelsNest.Pecan.Client.Issues.Store;
using SquirrelsNest.Pecan.Shared.Entities;
using System.Collections.Generic;

namespace SquirrelsNest.Pecan.Client.Issues.Reducers {
    public static class AddIssueReducer {
        [ReducerMethod]
        public static IssueState AddIssueSuccess( IssueState state, AddIssueSuccess action ) {
            var issueList = new List<SnCompositeIssue>( state.Issues ) { action.Issue };

            return new IssueState( issueList );
        }
    }
}
