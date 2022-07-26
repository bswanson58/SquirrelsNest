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
        projects: [],
        queryInfo: { hasPreviousPage: false, hasNextPage: false, loadedProjects: 0, totalProjects: 0 }
      }

    case APPEND_PROJECTS:
      const appendPayload = action as AppendProjects
      const newProjectList = [...state.projects, ...appendPayload.projects]

      return {
        ...state,
        projects: newProjectList,
        queryInfo: { ...appendPayload.queryInfo, loadedProjects: newProjectList.length }
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
