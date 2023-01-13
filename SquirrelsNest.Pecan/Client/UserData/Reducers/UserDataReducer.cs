using System;
using Fluxor;
using SquirrelsNest.Pecan.Client.UserData.Actions;
using SquirrelsNest.Pecan.Client.UserData.Store;

namespace SquirrelsNest.Pecan.Client.UserData.Reducers {
    // ReSharper disable once UnusedType.Global
    public static class UserDataReducer {
        [ReducerMethod( typeof( RequestUserDataAction ))]
        public static UserDataState RequestUserData( UserDataState state ) =>
        new( true, String.Empty, state.CurrentProjectId, 
             state.DisplayCompletedIssues, state.DisplayCompletedIssuesLast, state.DisplayOnlyMyAssignedIssues );

        [ReducerMethod]
        public static UserDataState UpdateUserData( UserDataState state, RequestUserDataSuccess action ) =>
            new( false, String.Empty, action.UserData.CurrentProjectId,
                 state.DisplayCompletedIssues, state.DisplayCompletedIssuesLast, state.DisplayOnlyMyAssignedIssues );
    }
}
