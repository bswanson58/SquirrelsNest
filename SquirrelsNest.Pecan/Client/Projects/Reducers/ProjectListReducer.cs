using System;
using System.Linq;
using Fluxor;
using SquirrelsNest.Pecan.Client.Projects.Actions;
using SquirrelsNest.Pecan.Client.Projects.Store;
using SquirrelsNest.Pecan.Shared.Entities;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace SquirrelsNest.Pecan.Client.Projects.Reducers {
    public static class ProjectListReducer {
        [ReducerMethod( typeof( GetProjectsAction ))]
        public static ProjectState ReduceGetProjectsAction( ProjectState state ) =>
            new ( true, String.Empty, state.Projects );

        [ReducerMethod]
        public static ProjectState ReduceGetProjectsSuccess( ProjectState state, GetProjectsSuccessAction action ) =>
            new ( false, String.Empty, action.Projects );

        [ReducerMethod]
        public static ProjectState ReducerGetProjectsFailure( ProjectState state, GetProjectsFailureAction action ) =>
            new ( false, action.Message, Enumerable.Empty<SnProject>());
    }
}
