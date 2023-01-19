using Fluxor;
using SquirrelsNest.Pecan.Client.Issues.Actions;
using SquirrelsNest.Pecan.Client.Issues.Store;

namespace SquirrelsNest.Pecan.Client.Issues.Reducers {
    // ReSharper disable once UnusedType.Global
    public static class PrepareForNewProjectReducer {
        [ReducerMethod]
        public static IssueState PrepareForNewProject( IssueState state, PrepareForNewProjectAction  action ) =>
        new( state.ApiCallInProgress, state.ApiCallMessage,
             action.Issues, action.PageInformation, action.ProjectId, action.CurrentDisplayPage );
    }
}
