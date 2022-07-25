import {Action} from '@ngrx/store'
import {AUTH_FAILED, AUTH_LOGIN, AUTH_LOGOUT, AUTH_REQUESTED, LoginCompleted} from './auth.actions'
import {AuthState, initialAuthState} from './auth.state'
import {getAuthenticationClaims, saveAuthenticationToken} from './jwtSupport'

export function authReducer( state: AuthState = initialAuthState, action: Action ): AuthState {
  switch( action.type ) {
    case AUTH_REQUESTED:
      return {
        ...state,
        loading: true
      }

    case AUTH_FAILED:
      return {
        ...state,
        loading: false
      }

    case AUTH_LOGIN:
      const payload = action as LoginCompleted

      saveAuthenticationToken( payload.authInfo )

      return {
        ...state,
        loading: false,
        expiration: payload.authInfo.expiration,
        token: payload.authInfo.token,
        userClaims: getAuthenticationClaims()
      }

    case AUTH_LOGOUT:
      return {
        ...state,
        loading: false,
        expiration: 0,
        token: '',
        userClaims: []
      }

    default: {
      return state
    }
  }
}
