import {Action} from '@ngrx/store'
import {ClIssue} from '../Data/graphQlTypes'
import {IssueQueryInfo} from './issues.state'

export const CLEAR_ISSUES = '[Issue] Clear Issues'
export const APPEND_ISSUES = '[Issue] Append Issues'
export const UPDATE_ISSUE = '[Issue] Update Issue'
export const SET_ISSUES_LOADING = '[Issue] SetLoading'
export const CLEAR_ISSUES_LOADING = '[Issue] Clear Loading'

export class ClearIssues implements Action {
  readonly type = CLEAR_ISSUES
}

export class AppendIssues implements Action {
  readonly type = APPEND_ISSUES

  constructor( public issues: ClIssue[], public queryInfo: IssueQueryInfo ) {
  }
}

export class UpdateIssue implements Action {
  readonly type = UPDATE_ISSUE

  constructor( public issue: ClIssue ) {
  }
}

export class SetIssuesLoading implements Action {
  readonly type = SET_ISSUES_LOADING
}

export class ClearIssuesLoading implements Action {
  readonly type = CLEAR_ISSUES_LOADING
}
