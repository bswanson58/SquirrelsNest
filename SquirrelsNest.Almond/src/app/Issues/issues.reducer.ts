import {Action} from '@ngrx/store'
import {initialIssueQueryInfo, initialIssueState, IssueState} from './issues.state'
import {
  APPEND_ISSUES, AppendIssues,
  CLEAR_ISSUES_LOADING,
  CLEAR_ISSUES,
  SET_ISSUES_LOADING,
  UPDATE_ISSUE, UpdateIssue, ADD_ISSUE, AddIssue, DELETE_ISSUE, DeleteIssue
} from './issues.actions'

export function issuesReducer( state: IssueState = initialIssueState, action: Action ): IssueState {
  switch( action.type ) {
    case ADD_ISSUE:
      const addPayload = action as AddIssue

      return {
        ...state,
        issues: [addPayload.newIssue, ...state.issues]
      }

    case APPEND_ISSUES:
      const appendPayload = action as AppendIssues
      const newIssueList = [...state.issues, ...appendPayload.issues]

      return {
        ...state,
        issues: newIssueList,
        queryInfo: { ...appendPayload.queryInfo, loadedIssues: newIssueList.length }
      }

    case CLEAR_ISSUES:
      return {
        ...state,
        issues: [],
        queryInfo: initialIssueQueryInfo
      }

    case DELETE_ISSUE:
      const deletePayload = action as DeleteIssue

      return {
        ...state,
        queryInfo: {
          ...state.queryInfo,
          loadedIssues: state.queryInfo.loadedIssues - 1,
          totalIssues: state.queryInfo.totalIssues - 1
        },
        issues: state.issues.filter( i => i.id !== deletePayload.issueId )
      }

    case UPDATE_ISSUE:
      const updatePayload = action as UpdateIssue
      const updatedIssueList = state.issues.map( i => i.id === updatePayload.issue.id ? updatePayload.issue : i )

      return {
        ...state,
        issues: updatedIssueList
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
