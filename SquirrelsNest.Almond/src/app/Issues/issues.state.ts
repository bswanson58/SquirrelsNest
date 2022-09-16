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
}

export const initialIssueState: IssueState = {
  issues: [],
  queryInfo: initialIssueQueryInfo,
}

export const getIssueState = createFeatureSelector<IssueState>( 'issues' )

export const getIssues = ( state: IssueState ) => state.issues
export const getIssueQueryState = ( state: IssueState ) => state.queryInfo
export const getServerHasMoreIssues = ( state: IssueState ) => state.queryInfo.hasNextPage
export const getTotalIssues = ( state: IssueState ) => state.queryInfo.totalIssues
export const getLoadedIssues = ( state: IssueState ) => state.queryInfo.loadedIssues
