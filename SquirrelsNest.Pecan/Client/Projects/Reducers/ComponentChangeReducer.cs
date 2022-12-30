using System;
using System.Collections.Generic;
using System.Linq;
using Fluxor;
using SquirrelsNest.Pecan.Client.Projects.Actions;
using SquirrelsNest.Pecan.Client.Projects.Store;
using SquirrelsNest.Pecan.Shared.Dto.Projects;
using SquirrelsNest.Pecan.Shared.Entities;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace SquirrelsNest.Pecan.Client.Projects.Reducers {
    public static class ComponentChangeReducer {
        [ReducerMethod( typeof( ComponentChangeSubmitAction ))]
        public static ProjectState ComponentChangeSubmitReducer( ProjectState state ) =>
            new ProjectState( true, String.Empty, state.Projects, state.CurrentProject );

        [ReducerMethod]
        public static ProjectState ComponentChangeSuccessReducer( ProjectState state, ComponentChangeSuccessAction action ) {
            var project = state.Projects.FirstOrDefault( p => p.Project.EntityId.Equals( action.Response.Component?.ProjectId ));
            var newComponents = new List<SnComponent>();

            if(( project != null ) &&
               ( action.Response.Component != null )) {
                switch ( action.Response.ChangeType ) {
                    case EntityChangeType.Add:
                        newComponents.AddRange( project.Components );
                        newComponents.Add( action.Response.Component );
                        break;

                    case EntityChangeType.Delete:
                        newComponents.AddRange( 
                            project.Components.Where( c => !c.EntityId.Equals( action.Response.Component.EntityId )));
                        break;

                    case EntityChangeType.Update:
                        newComponents.AddRange( 
                            project.Components.Select( 
                                c => c.EntityId.Equals( action.Response.Component.EntityId ) ? action.Response.Component : c ));
                        break;
                }

                project = project.With( newComponents );

                return new ProjectState( false, String.Empty, 
                                         state.Projects.Select( p => p.EntityId.Equals( project.EntityId ) ? project : p ), 
                                         state.CurrentProject?.EntityId == project.EntityId ? project : state.CurrentProject );
            }

            return state;
        }

        [ReducerMethod]
        public static ProjectState ComponentChangeFailureReducer( ProjectState state, ComponentChangeFailureAction action ) =>
            new ( false, action.Message, state.Projects, state.CurrentProject );
    }
}
