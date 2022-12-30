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
    public static class WorkflowStateChangeReducer {
        [ReducerMethod( typeof( WorkflowStateChangeSubmitAction ))]
        public static ProjectState WorkflowStateChangeSubmitReducer( ProjectState state ) =>
            new ( true, String.Empty, state.Projects, state.CurrentProject );

        [ReducerMethod]
        public static ProjectState WorkflowStateChangeSuccessReducer( ProjectState state, WorkflowStateChangeSuccessAction action ) {
            var project = state.Projects.FirstOrDefault( p => p.Project.EntityId.Equals( action.Response.WorkflowState?.ProjectId ));
            var newWorkflowStates = new List<SnWorkflowState>();

            if(( project != null ) &&
               ( action.Response.WorkflowState != null )) {
                switch ( action.Response.ChangeType ) {
                    case EntityChangeType.Add:
                        newWorkflowStates.AddRange( project.WorkflowStates );
                        newWorkflowStates.Add( action.Response.WorkflowState );
                        break;

                    case EntityChangeType.Delete:
                        newWorkflowStates.AddRange( 
                            project.WorkflowStates.Where( c => !c.EntityId.Equals( action.Response.WorkflowState.EntityId )));
                        break;

                    case EntityChangeType.Update:
                        newWorkflowStates.AddRange( 
                            project.WorkflowStates.Select( 
                                c => c.EntityId.Equals( action.Response.WorkflowState.EntityId ) ? action.Response.WorkflowState : c ));
                        break;
                }

                project = project.With( newWorkflowStates );

                return new ProjectState( false, String.Empty, 
                                         state.Projects.Select( p => p.EntityId.Equals( project.EntityId ) ? project : p ), 
                                         state.CurrentProject?.EntityId == project.EntityId ? project : state.CurrentProject );
            }

            return state;
        }

        [ReducerMethod]
        public static ProjectState WorkflowStateChangeFailureReducer( ProjectState state, WorkflowStateChangeFailureAction action ) =>
            new ( false, action.Message, state.Projects, state.CurrentProject );
    }
}
