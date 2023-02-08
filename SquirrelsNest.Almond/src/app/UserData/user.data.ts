import {eIssueDisplayStyle} from '../UI/ui.state'

export interface UserData {
  currentProject: string
  displayStyle: eIssueDisplayStyle
  displayCompletedIssues: boolean
  displayOnlyMyIssues: boolean
}
