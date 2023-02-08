import {createFeatureSelector} from '@ngrx/store'
import {ClUser} from '../Data/graphQlTypes'

export interface UserQueryInfo {
  hasNextPage: boolean,
  hasPreviousPage: boolean,
  loadedUsers: number,
  totalUsers: number
}

export const initialUserQueryInfo: UserQueryInfo = {
  hasNextPage: false,
  hasPreviousPage: false,
  loadedUsers: 0,
  totalUsers: 0
}

export interface UserState {
  users: ClUser[],
  queryInfo: UserQueryInfo,
  isLoading: boolean
}

export const initialUserState: UserState = {
  users: [],
  queryInfo: initialUserQueryInfo,
  isLoading: false
}

export const getUserState = createFeatureSelector<UserState>( 'users' )

export const getIsLoading = ( state: UserState ) => state.isLoading
export const getUsers = ( state: UserState ) => state.users
export const getUserQueryState = ( state: UserState ) => state.queryInfo
export const getServerHasMoreUsers = ( state: UserState ) => state.queryInfo.hasNextPage
export const getTotalUsers = ( state: UserState ) => state.queryInfo.totalUsers
export const getLoadedUsers = ( state: UserState ) => state.queryInfo.loadedUsers
