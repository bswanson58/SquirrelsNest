import {Action} from '@ngrx/store'
import {initialProjectQueryInfo, initialProjectState, ProjectState} from './project.state'
import {
  ADD_PROJECT, AddProject,
  APPEND_PROJECTS, AppendProjects,
  CLEAR_PROJECTS_LOADING,
  CLEAR_PROJECTS,
  DELETE_PROJECT, DeleteProject,
  SELECT_PROJECT, SelectProject,
  SET_PROJECTS_LOADING,
  UPDATE_PROJECT_DETAIL, UpdateProjectDetail,
  UPDATE_PROJECT, UpdateProject
} from './projects.actions'

export function projectsReducer( state: ProjectState = initialProjectState, action: Action ): ProjectState {
  switch( action.type ) {
    case ADD_PROJECT:
      const addPayload = action as AddProject

      return {
        ...state,
        projects: [addPayload.project, ...state.projects]
      }

    case UPDATE_PROJECT:
      const updatePayload = action as UpdateProject

      return {
        ...state,
        projects: state.projects.map( p => p.id === updatePayload.project.id ? updatePayload.project : p ),
        // if the selected project is the project being updated, also change it to trigger observables on the selected project.
        selectedProject: updatePayload.project.id === state.selectedProject?.id ? updatePayload.project : state.selectedProject
      }

    case UPDATE_PROJECT_DETAIL:
      const updateDetailPayload = action as UpdateProjectDetail

      return {
        ...state,
        projects: state.projects.map( p => p.id === updateDetailPayload.project.id ? updateDetailPayload.project : p ),
        // if the selected project is the project being updated, also change it to trigger observables on the selected project.
        selectedProject: updateDetailPayload.project.id === state.selectedProject?.id ? updateDetailPayload.project : state.selectedProject
      }

    case CLEAR_PROJECTS:
      return {
        ...state,
        projects: [],
        selectedProject: null,
        queryInfo: initialProjectQueryInfo
      }

    case DELETE_PROJECT:
      const deletePayload = action as DeleteProject

      return {
        ...state,
        projects: state.projects.filter( p => p.id !== deletePayload.projectId ),
        selectedProject: state.selectedProject?.id === deletePayload.projectId ? null : state.selectedProject
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
