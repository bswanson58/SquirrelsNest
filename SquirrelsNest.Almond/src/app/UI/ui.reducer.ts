import {Action} from '@ngrx/store'
import {TOGGLE_ISSUE_LIST_STYLE} from './ui.actions'
import {eIssueDisplayStyle, initialUiState, UiState} from './ui.state'

export function uiReducer( state: UiState = initialUiState, action: Action ): UiState {
  switch( action.type ) {
    case TOGGLE_ISSUE_LIST_STYLE:
      let newState = state.issueList.displayStyle

      switch( state.issueList.displayStyle ) {
        case eIssueDisplayStyle.FULL_DETAILS:
          newState = eIssueDisplayStyle.TITLE_DESCRIPTION
          break

        case eIssueDisplayStyle.TITLE_DESCRIPTION:
          newState = eIssueDisplayStyle.TITLE_ONLY
          break

        case eIssueDisplayStyle.TITLE_ONLY:
          newState = eIssueDisplayStyle.FULL_DETAILS
          break
      }

      return {
        ...state,
        issueList: {
          displayStyle: newState
        }
      }

    default: {
      return state
    }
  }
}
