import {Action} from '@ngrx/store'

export const TOGGLE_ISSUE_LIST_STYLE = '[UI] Toggle Issue List Style'
export const DISPLAY_ONLY_MY_ISSUES = '[UI] Display ONly My Issues'
export const DISPLAY_COMPLETED_ISSUES = '[UI] Display Completed Issues'

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
