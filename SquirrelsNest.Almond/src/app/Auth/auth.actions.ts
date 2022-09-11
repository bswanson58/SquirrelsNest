import {Action} from '@ngrx/store'
import {LoginPayload} from '../Data/graphQlTypes'

export const AUTH_REQUESTED = '[AUTH] Requested'
export const AUTH_FAILED = '[AUTH] Failed'
export const AUTH_LOGIN = '[AUTH] Login'
export const AUTH_LOGOUT = '[AUTH] Logout'

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
