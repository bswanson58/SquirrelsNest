import {Action} from '@ngrx/store'
import {
  CLEAR_ERROR,
  DISPLAY_COMPLETED_ISSUES,
  DISPLAY_ONLY_MY_ISSUES,
  DisplayCompletedIssues,
  DisplayOnlyMyIssues,
  REPORT_ERROR,
  ReportError,
  SERVICE_ENDED,
  SERVICE_STARTED,
  ServiceCallStarted,
  TOGGLE_ISSUE_LIST_STYLE
} from './ui.actions'
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
          ...state.issueList,
          displayStyle: newState
        }
      }

    case DISPLAY_COMPLETED_ISSUES:
      const completedState = action as DisplayCompletedIssues

      return {
        ...state,
        issueList: {
          ...state.issueList,
          displayCompletedIssues: completedState.state
        }
      }

    case DISPLAY_ONLY_MY_ISSUES:
      const myIssuesState = action as DisplayOnlyMyIssues

      return {
        ...state,
        issueList: {
          ...state.issueList,
          displayOnlyMyIssues: myIssuesState.state
        }
      }

    case REPORT_ERROR:
      const errorReport = action as ReportError

      return {
        ...state,
        errors: {
          ...state.errors,
          lastError: errorReport.errorMessage
        }
      }

    case CLEAR_ERROR:
      return {
        ...state,
        errors:{
          ...state.errors,
          lastError: ''
        }
      }

    case SERVICE_STARTED:
      const serviceStartedPayload = action as ServiceCallStarted

      return {
        ...state,
        service: {
          ...state.service,
          isActive: true,
          serviceActivity: serviceStartedPayload.activity
        }
      }

    case SERVICE_ENDED:
      return {
        ...state,
        service: {
          ...state.service,
          isActive: false,
          serviceActivity: ''
        }
      }

    default: {
      return state
    }
  }
}
