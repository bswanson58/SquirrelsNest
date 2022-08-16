import {Action} from '@ngrx/store'
import {initialProjectQueryInfo, initialProjectState, ProjectState} from './project.state'
import {
  APPEND_PROJECTS, AppendProjects,
  CLEAR_PROJECTS_LOADING,
  CLEAR_PROJECTS,
  SELECT_PROJECT, SelectProject,
  SET_PROJECTS_LOADING, ADD_PROJECT, AddProject, ADD_PROJECT_DETAIL, AddProjectDetail, DELETE_PROJECT, DeleteProject
} from './projects.actions'

export function projectsReducer( state: ProjectState = initialProjectState, action: Action ): ProjectState {
  switch( action.type ) {
    case ADD_PROJECT:
      const addPayload = action as AddProject

      return {
        ...state,
        projects: [addPayload.project, ...state.projects]
      }

    case ADD_PROJECT_DETAIL:
      const addDetailPayload = action as AddProjectDetail

      const retValue = {
        ...state,
        projects: state.projects.map( p => p.id === addDetailPayload.project.id ? addDetailPayload.project : p )
      }
      console.log(retValue.projects)
      return retValue

    case CLEAR_PROJECTS:
      return {
        ...state,
        projects: [],
        queryInfo: initialProjectQueryInfo
      }

    case DELETE_PROJECT:
      const deletePayload = action as DeleteProject

      return {
        ...state,
        projects: state.projects.filter( p => p.id !== deletePayload.projectId )
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
