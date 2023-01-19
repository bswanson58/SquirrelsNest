using System;
using System.Collections.Generic;
using System.Linq;
using Fluxor;
using SquirrelsNest.Pecan.Client.Issues.Actions;
using SquirrelsNest.Pecan.Client.Issues.Store;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.Issues.Reducers {
    // ReSharper disable once UnusedType.Global
    public static class UpdateIssueReducer {
        [ReducerMethod( typeof( UpdateIssueSubmit ))]
        public static IssueState UpdateIssueSubmit( IssueState state ) =>
            new ( true, String.Empty, state.Issues, state.PageInformation );

        [ReducerMethod]
        public static IssueState UpdateIssueSuccess( IssueState state, UpdateIssueSuccess action ) {
            var issues = new List<SnCompositeIssue>( 
                    state.Issues.Select( i => i.EntityId.Equals( action.Issue.EntityId ) ? action.Issue : i ));

            return new IssueState( false, String.Empty, issues, state.PageInformation );
        }

        [ReducerMethod]
        public static IssueState UpdateIssueFailure( IssueState state, UpdateIssueFailure action ) =>
            new ( false, action.Message, state.Issues, state.PageInformation );
    }
}
