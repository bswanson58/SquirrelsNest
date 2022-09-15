import {Action} from '@ngrx/store'

export const TOGGLE_ISSUE_LIST_STYLE = '[INTF] Toggle Issue List Style'
export const DISPLAY_ONLY_MY_ISSUES = '[INTF] Display Only My Issues'
export const DISPLAY_COMPLETED_ISSUES = '[INTF] Display Completed Issues'
export const REPORT_ERROR = '[INTF] Report Error'

export class ToggleIssueListStyle implements Action {
  readonly type = TOGGLE_ISSUE_LIST_STYLE
}

export class DisplayOnlyMyIssues implements Action {
  readonly type = DISPLAY_ONLY_MY_ISSUES

  constructor( public state: boolean ) {
  }
}

export class DisplayCompletedIssues implements Action {
  readonly type = DISPLAY_COMPLETED_ISSUES

  constructor( public state: boolean ) {
  }
}

export class ReportError implements Action {
  readonly type = REPORT_ERROR

  constructor( public errorMessage: string ) {
  }
}
