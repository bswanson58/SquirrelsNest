import {ActionReducerMap} from '@ngrx/store'
import {authReducer} from '../Auth/auth.reducer'
import {AuthState} from '../Auth/auth.state'
import {ProjectState} from '../Projects/project.state'
import {projectsReducer} from '../Projects/projects.reducer'

export interface AppState {
  auth: AuthState
  projects: ProjectState
}

export const appReducers: ActionReducerMap<AppState> = {
  auth: authReducer,
  projects: projectsReducer
}
