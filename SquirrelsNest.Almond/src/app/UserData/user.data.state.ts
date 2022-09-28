import {createFeatureSelector} from '@ngrx/store'
import {UserData} from './user.data'

export interface UserDataState {
  lastProject: string
}

export const initialUserDataState: UserDataState = {
  lastProject: ''
}

export const getUserDataState = createFeatureSelector<UserDataState>( 'userData' )

export const getLastProject = ( state: UserDataState ) => state.lastProject
export const getUserData = ( state: UserDataState ) : UserData => {
  return {
    currentProject: state.lastProject
  }
}
