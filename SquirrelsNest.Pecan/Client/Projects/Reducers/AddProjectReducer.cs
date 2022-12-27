using System;
using System.Collections.Generic;
using Fluxor;
using SquirrelsNest.Pecan.Client.Projects.Actions;
using SquirrelsNest.Pecan.Client.Projects.Store;
using SquirrelsNest.Pecan.Shared.Entities;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace SquirrelsNest.Pecan.Client.Projects.Reducers {
    public static class AddProjectReducer {
        [ReducerMethod( typeof( AddProjectAction ))]
        public static ProjectState ReduceAddProjectAction( ProjectState state ) => state;

        [ReducerMethod( typeof( AddProjectSubmitAction ))]
        public static ProjectState ReduceAddProjectSubmitAction( ProjectState state ) =>
            new ( true, String.Empty, state.Projects, state.CurrentProject );

        [ReducerMethod]
        public static ProjectState ReduceAddProjectSuccessAction( ProjectState state, AddProjectSuccess action ) {
            var projectList = new List<SnProject>( state.Projects ) { action.Project };

            return new ProjectState( false, String.Empty, projectList, state.CurrentProject );
        }

        [ReducerMethod]
        public static ProjectState ReduceAddProjectFailureAction( ProjectState state, AddProjectFailure action ) =>
            new ( false, action.Message, state.Projects, state.CurrentProject );
    }
}
