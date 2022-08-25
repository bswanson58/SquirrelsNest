import {Action} from '@ngrx/store'
import {ClUser} from '../Data/graphQlTypes'
import {UserQueryInfo} from './user.state'

export const ADD_USER = '[User] Add User'
export const APPEND_USERS = '[User] Append Users'
export const CLEAR_USERS = '[User] Clear Users'
export const SET_USERS_LOADING = '[User] Set Loading'
export const CLEAR_USERS_LOADING = '[User] Clear Loading'

export class AddUser implements Action {
  readonly type = ADD_USER

  constructor( public user: ClUser ) {
  }
}

export class AppendUsers implements Action {
  readonly type = APPEND_USERS

  constructor( public users: ClUser[], public queryInfo: UserQueryInfo ) {
  }
}

export class ClearUsers implements Action {
  readonly type = CLEAR_USERS
}

export class SetUsersLoading implements Action {
  readonly type = SET_USERS_LOADING
}

export class ClearUsersLoading implements Action {
  readonly type = CLEAR_USERS_LOADING
}
