import {createFeatureSelector} from '@ngrx/store'

export interface UserDataState {
  lastProject: string
}

export const initialUserDataState: UserDataState = {
  lastProject: ''
}

export const getUserDataState = createFeatureSelector<UserDataState>( 'userData' )

export const getLastProject = ( state: UserDataState ) => state.lastProject
