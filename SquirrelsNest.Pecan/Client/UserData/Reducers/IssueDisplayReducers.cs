﻿using Fluxor;
using SquirrelsNest.Pecan.Client.UserData.Actions;
using SquirrelsNest.Pecan.Client.UserData.Store;

namespace SquirrelsNest.Pecan.Client.UserData.Reducers {
    // ReSharper disable once UnusedType.Global
    public static class IssueDisplayReducers {
        [ReducerMethod]
        public static UserDataState IssueDisplayCompleted( UserDataState state, IssueDisplayCompleted action ) =>
            new( state.CurrentProjectId,
                 action.DisplayCompleted, state.DisplayCompletedIssuesLast, state.DisplayOnlyMyAssignedIssues,
                 state.IssueListDisplayStyle );

        [ReducerMethod]
        public static UserDataState IssueDisplayCompletedLast( UserDataState state, IssueDisplayCompletedLast action ) =>
            new( state.CurrentProjectId,
                 state.DisplayCompletedIssues, action.DisplayCompletedLast, state.DisplayOnlyMyAssignedIssues,
                 state.IssueListDisplayStyle );

        [ReducerMethod]
        public static UserDataState IssueDisplayMyAssigned( UserDataState state, IssueDisplayMyAssigned action ) =>
            new( state.CurrentProjectId,
                 state.DisplayCompletedIssues, state.DisplayCompletedIssuesLast, action.DisplayMyAssigned,
                 state.IssueListDisplayStyle );

        [ReducerMethod]
        public static UserDataState IssueListDisplayStyle( UserDataState state, IssueListDisplayStyleAction action ) =>
            new( state.CurrentProjectId,
                 state.DisplayCompletedIssues, state.DisplayCompletedIssuesLast, state.DisplayOnlyMyAssignedIssues,
                 action.IssueListDisplayStyle );
    }
}
