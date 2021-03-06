import {Action} from '@ngrx/store'
import {initialIssueQueryInfo, initialIssueState, IssueState} from './issues.state'
import {
  APPEND_ISSUES, AppendIssues,
  CLEAR_ISSUES_LOADING,
  CLEAR_ISSUES,
  SET_ISSUES_LOADING
} from './issues.actions'

export function issuesReducer( state: IssueState = initialIssueState, action: Action ): IssueState {
  switch( action.type ) {
    case CLEAR_ISSUES:
      return {
        ...state,
        issues: [],
        queryInfo: initialIssueQueryInfo
      }

    case APPEND_ISSUES:
      const appendPayload = action as AppendIssues
      const newIssueList = [...state.issues, ...appendPayload.issues]

      return {
        ...state,
        issues: newIssueList,
        queryInfo: { ...appendPayload.queryInfo, loadedIssues: newIssueList.length }
      }

    case SET_ISSUES_LOADING:
      return {
        ...state,
        isLoading: true
      }

    case CLEAR_ISSUES_LOADING:
      return {
        ...state,
        isLoading: false
      }

    default: {
      return state
    }
  }
}
