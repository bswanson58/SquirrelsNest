import {Action} from '@ngrx/store'
import {UserData} from './user.data'

export const UPDATE_USER_DATA = '[UDAT] Update User Data'
export const CLEAR_USER_DATA = '[UDAT] Clear User Data'

export class UpdateUserData implements Action {
  readonly type = UPDATE_USER_DATA

  constructor( public userData: UserData ) {
  }
}

export class ClearUserData implements Action {
  readonly type = CLEAR_USER_DATA
}
