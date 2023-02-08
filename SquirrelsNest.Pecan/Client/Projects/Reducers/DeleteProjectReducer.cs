using System;
using Fluxor;
using SquirrelsNest.Pecan.Shared.Entities;
using System.Collections.Generic;
using System.Linq;
using SquirrelsNest.Pecan.Client.Projects.Actions;
using SquirrelsNest.Pecan.Client.Projects.Store;

namespace SquirrelsNest.Pecan.Client.Projects.Reducers {
    // ReSharper disable once UnusedType.Global
    public static class DeleteProjectReducer {
        [ReducerMethod( typeof( DeleteProjectSubmit ))]
        public static ProjectState DeleteProjectSubmit( ProjectState state ) =>
            new ( true, String.Empty, state.Projects, state.CurrentProject );

        [ReducerMethod]
        public static ProjectState DeleteProjectSuccess( ProjectState state, DeleteProjectSuccess action ) {
            var projects = new List<SnCompositeProject>( 
                state.Projects.Where( p => !p.EntityId.Equals( action.Project.EntityId )));
            var currentProject = state.CurrentProject != null && 
                                 state.CurrentProject.EntityId.Equals( action.Project.EntityId ) ? null : state.CurrentProject;

            return new ( false, String.Empty, projects, currentProject );
        }

        [ReducerMethod]
        public static ProjectState DeleteProjectFailure( ProjectState state, DeleteProjectFailure action ) =>
            new ( false, action.Message, state.Projects, state.CurrentProject );
    }
}
