import {Action} from '@ngrx/store'
import {UPDATE_USER_DATA, UpdateUserData} from './user.data.actions'
import {initialUserDataState, UserDataState} from './user.data.state'

export function userDataReducer( state: UserDataState = initialUserDataState, action: Action ): UserDataState {
  switch( action.type ) {
    case UPDATE_USER_DATA:
      const updateUserData = action as UpdateUserData

      return {
        ...state,
        lastProject : updateUserData.userData.currentProject
      }

    default:
      return state
  }
}
