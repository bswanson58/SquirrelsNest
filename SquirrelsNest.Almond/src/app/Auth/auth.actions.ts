import {Action} from '@ngrx/store'
import {LoginPayload} from '../Data/graphQlTypes'

export const AUTH_REQUESTED = '[Auth] Requested'
export const AUTH_FAILED = '[Auth] Failed'
export const AUTH_LOGIN = '[Auth] Login'
export const AUTH_LOGOUT = '[Auth] Logout'

export class AuthRequested implements Action {
  readonly type = AUTH_REQUESTED
}

export class AuthFailed implements Action {
  readonly type = AUTH_FAILED
}

export class LoginCompleted implements Action {
  readonly type = AUTH_LOGIN

  constructor( public authInfo: LoginPayload ) {
  }
}

export class Logout implements Action {
  readonly type = AUTH_LOGOUT
}

export type AuthActions = AuthRequested | AuthFailed | LoginCompleted | Logout
