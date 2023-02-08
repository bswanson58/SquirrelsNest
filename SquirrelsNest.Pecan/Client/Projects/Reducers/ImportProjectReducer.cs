using System;
using System.Collections.Generic;
using Fluxor;
using SquirrelsNest.Pecan.Client.Projects.Actions;
using SquirrelsNest.Pecan.Client.Projects.Store;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.Projects.Reducers {
    // ReSharper disable once UnusedType.Global
    public static class ImportProjectReducer {
        [ReducerMethod( typeof( ImportProjectSubmit ))]
        public static ProjectState ImportProjectSubmit( ProjectState state ) =>
            new( true, String.Empty, state.Projects, state.CurrentProject );

        [ReducerMethod]
        public static ProjectState ImportProjectSuccess( ProjectState state, ImportProjectSuccess action ) {
            var projects = new List<SnCompositeProject>( state.Projects ) { action.Project };

            return new( false, String.Empty, projects, state.CurrentProject );
        }

        [ReducerMethod]
        public static ProjectState ImportProjectFailure( ProjectState state, ImportProjectFailure action ) =>
            new( false, action.Message, state.Projects, state.CurrentProject );
    }
}
