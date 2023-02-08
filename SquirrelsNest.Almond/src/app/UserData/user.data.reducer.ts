import {Action} from '@ngrx/store'
import {CLEAR_USER_DATA, UPDATE_USER_DATA, UpdateUserData} from './user.data.actions'
import {initialUserDataState, UserDataState} from './user.data.state'

export function userDataReducer( state: UserDataState = initialUserDataState, action: Action ): UserDataState {
  switch( action.type ) {
    case UPDATE_USER_DATA:
      const updateUserData = action as UpdateUserData

      return {
        lastProject: updateUserData.userData.currentProject,
        displayStyle: updateUserData.userData.displayStyle,
        displayOnlyMyIssues: updateUserData.userData.displayOnlyMyIssues,
        displayCompletedIssues: updateUserData.userData.displayCompletedIssues,
      }

    case CLEAR_USER_DATA:
      return {
        lastProject: initialUserDataState.lastProject,
        displayStyle: initialUserDataState.displayStyle,
        displayOnlyMyIssues: initialUserDataState.displayOnlyMyIssues,
        displayCompletedIssues: initialUserDataState.displayCompletedIssues,
      }

    default:
      return state
  }
}
