using Fluxor;
using SquirrelsNest.Pecan.Client.Projects.Actions;
using SquirrelsNest.Pecan.Client.Projects.Store;

namespace SquirrelsNest.Pecan.Client.Projects.Reducers {
    public static class SetCurrentProjectReducer {
        [ReducerMethod]
        public static ProjectState ReduceSetCurrentProject( ProjectState state, SetCurrentProjectAction action ) =>
            new( state.ApiCallInProgress, state.ApiCallMessage, state.Projects, action.Project );
    }
}
