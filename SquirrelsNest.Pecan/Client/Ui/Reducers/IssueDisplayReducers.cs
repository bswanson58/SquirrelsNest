using Fluxor;
using SquirrelsNest.Pecan.Client.Ui.Actions;
using SquirrelsNest.Pecan.Client.Ui.Store;

namespace SquirrelsNest.Pecan.Client.Ui.Reducers {
    // ReSharper disable once UnusedType.Global
    public static class IssueDisplayReducers {
        [ReducerMethod]
        public static UiState IssueDisplayCompleted( UiState state, IssueDisplayCompleted action ) =>
            new( action.DisplayCompleted, state.DisplayCompletedIssuesLast, state.DisplayOnlyMyAssignedIssues );

        [ReducerMethod]
        public static UiState IssueDisplayCompletedLast( UiState state, IssueDisplayCompletedLast action ) =>
            new( state.DisplayCompletedIssues, action.DisplayCompletedLast, state.DisplayOnlyMyAssignedIssues );
        
        [ReducerMethod]
        public static UiState IssueDisplayMyAssigned( UiState state, IssueDisplayMyAssigned action ) =>
            new( state.DisplayCompletedIssues, state.DisplayCompletedIssuesLast, action.DisplayMyAssigned );
    }
}
