import {Action} from '@ngrx/store'
import {initialProjectState, ProjectState} from './project.state'
import {
  APPEND_PROJECTS, AppendProjects,
  CLEAR_PROJECTS_LOADING,
  CLEAR_PROJECTS,
  SELECT_PROJECT, SelectProject,
  SET_PROJECTS_LOADING
} from './projects.actions'

export function projectsReducer( state: ProjectState = initialProjectState, action: Action ): ProjectState {
  switch( action.type ) {
    case CLEAR_PROJECTS:
      return {
        ...state,
        projects: []
      }

    case APPEND_PROJECTS:
      const appendPayload = action as AppendProjects

      return {
        ...state,
        projects: [...state.projects, ...appendPayload.projects]
      }

    case SELECT_PROJECT:
      const selectPayload = action as SelectProject
      return {
        ...state,
        selectedProject: selectPayload.selectedProject
      }

    case SET_PROJECTS_LOADING:
      return {
        ...state,
        isLoading: true
      }

    case CLEAR_PROJECTS_LOADING:
      return {
        ...state,
        isLoading: false
      }

    default: {
      return state
    }
  }
}
