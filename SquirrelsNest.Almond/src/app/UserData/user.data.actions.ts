import {Action} from '@ngrx/store'
import {UserData} from './user.data'

export const UPDATE_USER_DATA = '[UDAT] Update User Data'

export class UpdateUserData implements Action {
  readonly type = UPDATE_USER_DATA

  constructor( public userData: UserData ) {
  }
}
