import {Action} from '@ngrx/store'
import {ClUser} from '../Data/graphQlTypes'
import {UserQueryInfo} from './user.state'

export const ADD_USER = '[USER] Add User'
export const APPEND_USERS = '[USER] Append Users'
export const CLEAR_USERS = '[USER] Clear Users'
export const DELETE_USER = '[USER] Delete User'
export const UPDATE_USER = '[USER] Update User'
export const SET_USERS_LOADING = '[USER] Set Loading'
export const CLEAR_USERS_LOADING = '[USER] Clear Loading'

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

export class DeleteUser implements Action {
  readonly type = DELETE_USER

  constructor( public user: ClUser ) {
  }
}

export class UpdateUser implements Action {
  readonly type = UPDATE_USER

  constructor( public user: ClUser ) {
  }
}

export class SetUsersLoading implements Action {
  readonly type = SET_USERS_LOADING
}

export class ClearUsersLoading implements Action {
  readonly type = CLEAR_USERS_LOADING
}
