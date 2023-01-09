using System;
using System.Collections.Generic;
using System.Linq;
using Fluxor;
using SquirrelsNest.Pecan.Client.Issues.Actions;
using SquirrelsNest.Pecan.Client.Issues.Store;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.Issues.Reducers {
    // ReSharper disable once UnusedType.Global
    public class DeleteIssueReducer {
        [ReducerMethod( typeof( DeleteIssueSubmitAction ))]
        public static IssueState DeleteIssueSubmit( IssueState state ) =>
            new ( true, String.Empty, state.Issues );

        [ReducerMethod]
        public static IssueState DeleteIssueSuccess( IssueState state, DeleteIssueSuccess action ) {
            var issues = new List<SnCompositeIssue>( 
                state.Issues.Where( i => !i.EntityId.Equals( action.Issue.EntityId )));

            return new IssueState( false, String.Empty, issues );
        }

        [ReducerMethod]
        public static IssueState DeleteIssueFailure( IssueState state, DeleteIssueFailure action ) =>
            new ( false, action.Message, state.Issues );
    }
}
