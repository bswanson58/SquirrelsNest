import {ActionReducerMap} from '@ngrx/store'
import {authReducer} from '../Auth/auth.reducer'
import {AuthState} from '../Auth/auth.state'

export interface AppState {
  auth: AuthState;
}

export const appReducers: ActionReducerMap<AppState> = {
  auth: authReducer
}
