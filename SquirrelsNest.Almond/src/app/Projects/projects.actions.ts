import {Action} from '@ngrx/store'
import {ClProject, ClProjectTemplate} from '../Data/graphQlTypes'
import {ProjectQueryInfo} from './project.state'

export const ADD_PROJECT = '[PROJ] Add Project'
export const CLEAR_PROJECTS = '[PROJ] Clear Projects'
export const DELETE_PROJECT = '[PROJ] Delete Project'
export const APPEND_PROJECTS = '[PROJ] Append Projects'
export const UPDATE_PROJECT = '[PROJ] Update Project'
export const UPDATE_PROJECT_DETAIL = '[PROJ] Update Project Detail'
export const SELECT_PROJECT = '[PROJ] Select Project'
export const SET_PROJECTS_LOADING = '[PROJ] SetLoading'
export const CLEAR_PROJECTS_LOADING = '[PROJ] Clear Loading'
export const UPDATE_PROJECT_TEMPLATES = '[PROJ] Update Templates'

export class AddProject implements Action {
  readonly type = ADD_PROJECT

  constructor( public project: ClProject ) {
  }
}

export class UpdateProject implements Action {
  readonly type = UPDATE_PROJECT

  constructor( public project: ClProject ) {
  }
}

export class UpdateProjectDetail implements Action {
  readonly type = UPDATE_PROJECT_DETAIL

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

export class UpdateTemplates implements Action {
  readonly type = UPDATE_PROJECT_TEMPLATES

  constructor( public templates: ClProjectTemplate[] ) {
  }
}

export class SetProjectLoading implements Action {
  readonly type = SET_PROJECTS_LOADING
}

export class ClearProjectLoading implements Action {
  readonly type = CLEAR_PROJECTS_LOADING
}
