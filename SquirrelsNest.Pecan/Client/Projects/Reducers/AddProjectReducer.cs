using System;
using System.Collections.Generic;
using Fluxor;
using SquirrelsNest.Pecan.Client.Projects.Actions;
using SquirrelsNest.Pecan.Client.Projects.Store;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.Projects.Reducers {
    public static class AddProjectReducer {
        [ReducerMethod( typeof( AddProjectAction ))]
        public static ProjectState ReduceAddProjectAction( ProjectState state ) => state;

        [ReducerMethod( typeof( AddProjectSubmitAction ))]
        public static ProjectState ReduceAddProjectSubmitAction( ProjectState state ) =>
            new ProjectState( true, String.Empty, state.Projects );

        [ReducerMethod]
        public static ProjectState ReduceAddProjectSuccessAction( ProjectState state, AddProjectSuccess action ) {
            var projectList = new List<SnProject>( state.Projects ) { action.Project };

            return new ProjectState( false, String.Empty, projectList );
        }

        [ReducerMethod]
        public static ProjectState ReduceAddProjectFailureAction( ProjectState state, AddProjectFailure action ) =>
            new ProjectState( false, action.Message, state.Projects );
    }
}
