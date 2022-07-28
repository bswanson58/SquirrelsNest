import {ActionReducerMap} from '@ngrx/store'
import {authReducer} from '../Auth/auth.reducer'
import {AuthState} from '../Auth/auth.state'
import {issuesReducer} from '../Issues/issues.reducer'
import {IssueState} from '../Issues/issues.state'
import {ProjectState} from '../Projects/project.state'
import {projectsReducer} from '../Projects/projects.reducer'
import {uiReducer} from '../UI/ui.reducer'
import {UiState} from '../UI/ui.state'

export interface AppState {
  auth: AuthState
  projects: ProjectState
  issues: IssueState
  ui: UiState
}

export const appReducers: ActionReducerMap<AppState> = {
  auth: authReducer,
  projects: projectsReducer,
  issues: issuesReducer,
  ui: uiReducer
}
