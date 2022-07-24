import {Action} from '@ngrx/store'

export const AUTH_SET_AUTHENTICATED = '[Auth] Set Authenticated'
export const AUTH_SET_UNAUTHENTICATED = '[Auth] Set Unauthenticated'

export class SetAuthenticated implements Action {
  readonly type = AUTH_SET_AUTHENTICATED
}

export class SetUnauthenticated implements Action {
  readonly type = AUTH_SET_UNAUTHENTICATED
}

export type AuthActions = SetAuthenticated | SetUnauthenticated;
