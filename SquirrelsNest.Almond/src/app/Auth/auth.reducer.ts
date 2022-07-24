import {Action} from '@ngrx/store'
import {AUTH_SET_AUTHENTICATED, AUTH_SET_UNAUTHENTICATED} from './auth.actions'
import {AuthState, initialAuthState} from './auth.state'

export function authReducer( state: AuthState = initialAuthState, action: Action ): AuthState {
  switch( action.type ) {
    case AUTH_SET_AUTHENTICATED:
      return {
        ...state,
        loading: false
      }
    case AUTH_SET_UNAUTHENTICATED:
      return {
        ...state,
        loading: true
      }
    default: {
      return state
    }
  }
}
