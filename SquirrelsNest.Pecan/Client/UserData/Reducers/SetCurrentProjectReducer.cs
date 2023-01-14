using Fluxor;
using SquirrelsNest.Pecan.Client.UserData.Actions;
using SquirrelsNest.Pecan.Client.UserData.Store;

namespace SquirrelsNest.Pecan.Client.UserData.Reducers {
    // ReSharper disable once UnusedType.Global
    public class SetCurrentProjectReducer {
        [ReducerMethod]
        public static UserDataState SetCurrentProject( UserDataState state, UserDataSetCurrentProjectAction action ) =>
            new( action.CurrentProjectId,
                 state.DisplayCompletedIssues, state.DisplayCompletedIssuesLast, state.DisplayOnlyMyAssignedIssues );
    }
}
