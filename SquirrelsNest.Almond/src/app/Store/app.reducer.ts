import {ActionReducerMap} from '@ngrx/store'
import {authReducer} from '../Auth/auth.reducer'
import {AuthState} from '../Auth/auth.state'
import {issuesReducer} from '../Issues/issues.reducer'
import {IssueState} from '../Issues/issues.state'
import {ProjectState} from '../Projects/project.state'
import {projectsReducer} from '../Projects/projects.reducer'
import {uiReducer} from '../UI/ui.reducer'
import {UiState} from '../UI/ui.state'
import {userDataReducer} from '../UserData/user.data.reducer'
import {UserDataState} from '../UserData/user.data.state'
import {usersReducer} from '../Users/user.reducer'
import {UserState} from '../Users/user.state'

export interface AppState {
  auth: AuthState
  projects: ProjectState
  issues: IssueState
  users: UserState,
  userData: UserDataState,
  ui: UiState
}

export const appReducers: ActionReducerMap<AppState> = {
  auth: authReducer,
  projects: projectsReducer,
  issues: issuesReducer,
  users: usersReducer,
  userData: userDataReducer,
  ui: uiReducer
}
