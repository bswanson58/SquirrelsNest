using Fluxor;
using SquirrelsNest.Pecan.Client.UserData.Actions;
using SquirrelsNest.Pecan.Client.UserData.Store;

namespace SquirrelsNest.Pecan.Client.UserData.Reducers {
    // ReSharper disable once UnusedType.Global
    public static class UserDataReducer {
        [ReducerMethod]
        public static UserDataState UpdateUserData( UserDataState state, RequestUserDataSuccess action ) =>
            new( action.UserData.CurrentProjectId,
                 action.UserData.DisplayCompletedIssues, 
                 action.UserData.DisplayCompletedIssuesLast,
                 action.UserData.DisplayOnlyMyAssignedIssues );
    }
}
