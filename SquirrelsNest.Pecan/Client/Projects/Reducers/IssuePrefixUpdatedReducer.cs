using System.Collections.Generic;
using System.Linq;
using Fluxor;
using SquirrelsNest.Pecan.Client.Issues.Actions;
using SquirrelsNest.Pecan.Client.Projects.Store;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.Projects.Reducers {
    // ReSharper disable once UnusedType.Global
    public static class IssuePrefixUpdatedReducer {
        [ReducerMethod]
        public static ProjectState IssueAddedToProject( ProjectState state, AddIssueSuccess action ) {
            var projectList = new List<SnCompositeProject>( 
                state.Projects.Select( p => p.EntityId.Equals( action.Project.EntityId ) ? action.Project : p ));
            var currentProject = state.CurrentProject != null && 
                                 state.CurrentProject.EntityId.Equals( action.Project.EntityId ) ? action.Project : state.CurrentProject;

            return new( state.ApiCallInProgress, state.ApiCallMessage, projectList, currentProject );
        }
    }
}
