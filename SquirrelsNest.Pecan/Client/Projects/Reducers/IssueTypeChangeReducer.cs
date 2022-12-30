using System;
using Fluxor;
using SquirrelsNest.Pecan.Client.Projects.Actions;
using SquirrelsNest.Pecan.Client.Projects.Store;
using SquirrelsNest.Pecan.Shared.Dto.Projects;
using SquirrelsNest.Pecan.Shared.Entities;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace SquirrelsNest.Pecan.Client.Projects.Reducers {
    public static class IssueTypeChangeReducer {
        [ReducerMethod( typeof( IssueTypeChangeSubmitAction ))]
        public static ProjectState IssueTypeChangeSubmitReducer( ProjectState state ) =>
            new ( true, String.Empty, state.Projects, state.CurrentProject );

        [ReducerMethod]
        public static ProjectState IssueTypeChangeSuccessReducer( ProjectState state, IssueTypeChangeSuccessAction action ) {
            var project = state.Projects.FirstOrDefault( p => p.Project.EntityId.Equals( action.Response.IssueType?.ProjectId ));
            var newIssueTypes = new List<SnIssueType>();

            if(( project != null ) &&
               ( action.Response.IssueType != null )) {
                switch ( action.Response.ChangeType ) {
                    case EntityChangeType.Add:
                        newIssueTypes.AddRange( project.IssueTypes );
                        newIssueTypes.Add( action.Response.IssueType );
                        break;

                    case EntityChangeType.Delete:
                        newIssueTypes.AddRange( 
                            project.IssueTypes.Where( c => !c.EntityId.Equals( action.Response.IssueType.EntityId )));
                        break;

                    case EntityChangeType.Update:
                        newIssueTypes.AddRange( 
                            project.IssueTypes.Select( 
                                c => c.EntityId.Equals( action.Response.IssueType.EntityId ) ? action.Response.IssueType : c ));
                        break;
                }

                project = project.With( newIssueTypes );

                return new ProjectState( false, String.Empty, 
                                         state.Projects.Select( p => p.EntityId.Equals( project.EntityId ) ? project : p ), 
                                         state.CurrentProject?.EntityId == project.EntityId ? project : state.CurrentProject );
            }

            return state;
        }

        [ReducerMethod]
        public static ProjectState IssueTypeChangeFailureReducer( ProjectState state, IssueTypeChangeFailureAction action ) =>
            new ( false, action.Message, state.Projects, state.CurrentProject );
    }
}
