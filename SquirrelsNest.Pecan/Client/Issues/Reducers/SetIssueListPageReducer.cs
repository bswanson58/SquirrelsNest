using Fluxor;
using SquirrelsNest.Pecan.Client.Issues.Actions;
using SquirrelsNest.Pecan.Client.Issues.Store;

namespace SquirrelsNest.Pecan.Client.Issues.Reducers {
    // ReSharper disable once UnusedType.Global
    public static class SetIssueListPageReducer {
        [ReducerMethod]
        public static IssueState SetIssueListPage( IssueState state, SetIssueListPageAction action ) =>
        new( state.ApiCallInProgress, state.ApiCallMessage,
             state.Issues, state.PageInformation, state.CurrentProjectId, action.PageNumber );
    }
}
