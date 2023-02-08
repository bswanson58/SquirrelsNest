import {Action} from '@ngrx/store'
import {ClIssue} from '../Data/graphQlTypes'
import {IssueQueryInfo} from './issues.state'

export const ADD_ISSUE = '[ISSU] Add Issue'
export const CLEAR_ISSUES = '[ISSU] Clear Issues'
export const DELETE_ISSUE = '[ISSU] Delete Issue'
export const APPEND_ISSUES = '[ISSU] Append Issues'
export const UPDATE_ISSUE = '[ISSU] Update Issue'

export class AddIssue implements Action {
  readonly type = ADD_ISSUE

  constructor( public newIssue: ClIssue ) {
  }
}

export class AppendIssues implements Action {
  readonly type = APPEND_ISSUES

  constructor( public issues: ClIssue[], public queryInfo: IssueQueryInfo ) {
  }
}

export class ClearIssues implements Action {
  readonly type = CLEAR_ISSUES
}

export class DeleteIssue implements Action {
  readonly type = DELETE_ISSUE

  constructor( public issueId: string ) {
  }
}

export class UpdateIssue implements Action {
  readonly type = UPDATE_ISSUE

  constructor( public issue: ClIssue ) {
  }
}
