import {Action} from '@ngrx/store'

export const TOGGLE_ISSUE_LIST_STYLE = '[UI] Toggle Issue List Style'

export class ToggleIssueListStyle implements Action {
  readonly type = TOGGLE_ISSUE_LIST_STYLE
}
