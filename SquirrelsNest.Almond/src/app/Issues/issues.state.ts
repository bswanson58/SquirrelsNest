import {createFeatureSelector} from '@ngrx/store'
import {ClIssue} from '../Data/graphQlTypes'

export interface IssueQueryInfo {
  hasNextPage: boolean,
  hasPreviousPage: boolean,
  loadedIssues: number,
  totalIssues: number
}

export const initialIssueQueryInfo: IssueQueryInfo = {
  hasNextPage: false,
  hasPreviousPage: false,
  loadedIssues: 0,
  totalIssues: 0
}

export interface IssueState {
  issues: ClIssue[],
  queryInfo: IssueQueryInfo,
  isLoading: boolean
}

export const initialIssueState: IssueState = {
  issues: [],
  queryInfo: initialIssueQueryInfo,
  isLoading: false
}

export const getIssueState = createFeatureSelector<IssueState>( 'issues' )

export const getIsLoading = ( state: IssueState ) => state.isLoading
export const getIssues = ( state: IssueState ) => state.issues
export const getIssueQueryState = ( state: IssueState ) => state.queryInfo
export const getServerHasMoreIssues = ( state: IssueState ) => state.queryInfo.hasNextPage
