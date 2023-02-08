using System;
using System.Collections.Generic;
using System.Linq;
using Fluxor;
using SquirrelsNest.Pecan.Client.Projects.Actions;
using SquirrelsNest.Pecan.Client.Projects.Store;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.Projects.Reducers {
    // ReSharper disable once UnusedType.Global
    public static class EditProjectReducer {
        [ReducerMethod( typeof( EditProjectSubmit ))]
        public static ProjectState EditProjectSubmit( ProjectState state ) =>
            new ( true, String.Empty, state.Projects, state.CurrentProject );

        [ReducerMethod]
        public static ProjectState EditProjectSuccess( ProjectState state, EditProjectSuccess action ) {
            var projectList = new List<SnCompositeProject>( 
                state.Projects.Select( p => p.EntityId.Equals( action.Project.EntityId ) ? action.Project : p ));
            var currentProject = state.CurrentProject != null && 
                                 state.CurrentProject.EntityId.Equals( action.Project.EntityId ) ? action.Project : state.CurrentProject;

            return new ProjectState( false, String.Empty, projectList, currentProject );
        }

        [ReducerMethod]
        public static ProjectState EditProjectFailure( ProjectState state, EditProjectFailure action ) =>
            new ( false, action.Message, state.Projects, state.CurrentProject );
    }
}
