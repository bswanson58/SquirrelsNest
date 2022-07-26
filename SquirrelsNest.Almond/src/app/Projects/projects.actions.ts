import {Action} from '@ngrx/store'
import {ClProject} from '../Data/graphQlTypes'
import {ProjectQueryInfo} from './project.state'

export const CLEAR_PROJECTS = '[Project] Clear Projects'
export const APPEND_PROJECTS = '[Project] Append Projects'
export const SELECT_PROJECT = '[Project] Select Project'
export const SET_PROJECTS_LOADING = '[Project] SetLoading'
export const CLEAR_PROJECTS_LOADING = '[Project] Clear Loading'

export class ClearProjects implements Action {
  readonly type = CLEAR_PROJECTS
}

export class AppendProjects implements Action {
  readonly type = APPEND_PROJECTS

  constructor( public projects: ClProject[], public queryInfo: ProjectQueryInfo ) {
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
