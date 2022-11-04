import {createFeatureSelector} from '@ngrx/store'
import {eIssueDisplayStyle} from '../UI/ui.state'
import {UserData} from './user.data'

export interface UserDataState {
  lastProject: string,
  displayStyle: eIssueDisplayStyle,
  displayOnlyMyIssues: boolean,
  displayCompletedIssues: boolean,
}

export const initialUserDataState: UserDataState = {
  lastProject: '',
  displayStyle: eIssueDisplayStyle.FULL_DETAILS,
  displayOnlyMyIssues: false,
  displayCompletedIssues: true
}

export const getUserDataState = createFeatureSelector<UserDataState>( 'userData' )

export const getLastProject = ( state: UserDataState ) => state.lastProject
export const getUserData = ( state: UserDataState ): UserData => {
  return {
    ...state,
    currentProject: state.lastProject
  }
}
