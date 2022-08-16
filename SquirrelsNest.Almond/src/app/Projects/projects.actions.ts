import {Action} from '@ngrx/store'
import {AddProjectPayload, ClProject} from '../Data/graphQlTypes'
import {ProjectQueryInfo} from './project.state'

export const ADD_PROJECT = '[Project] Add Project'
export const ADD_PROJECT_DETAIL = '[Project] Add Project Detail'
export const CLEAR_PROJECTS = '[Project] Clear Projects'
export const DELETE_PROJECT = '[Project] Delete Project'
export const APPEND_PROJECTS = '[Project] Append Projects'
export const SELECT_PROJECT = '[Project] Select Project'
export const SET_PROJECTS_LOADING = '[Project] SetLoading'
export const CLEAR_PROJECTS_LOADING = '[Project] Clear Loading'

export class AddProject implements Action {
  readonly type = ADD_PROJECT

  constructor( public project: ClProject ) {
  }
}

export class AddProjectDetail implements Action {
  readonly type = ADD_PROJECT_DETAIL

  constructor( public project: ClProject ) {
  }
}

export class ClearProjects implements Action {
  readonly type = CLEAR_PROJECTS
}

export class AppendProjects implements Action {
  readonly type = APPEND_PROJECTS

  constructor( public projects: ClProject[], public queryInfo: ProjectQueryInfo ) {
  }
}

export class DeleteProject implements Action {
  readonly type = DELETE_PROJECT

  constructor( public projectId: string ) {
  }
}

export class SelectProject implements Action {
  readonly type = SELECT_PROJECT

  constructor( public selectedProject: ClProject ) {
  }
}

export class SetProjectLoading implements Action {
  readonly type = SET_PROJECTS_LOADING
}

export class ClearProjectLoading implements Action {
  readonly type = CLEAR_PROJECTS_LOADING
}
