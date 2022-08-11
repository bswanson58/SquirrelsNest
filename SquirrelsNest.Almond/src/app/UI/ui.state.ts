import {createFeatureSelector} from '@ngrx/store'

export enum eIssueDisplayStyle { TITLE_ONLY, TITLE_DESCRIPTION, FULL_DETAILS }

export interface UiState {
  issueList: {
    displayStyle: eIssueDisplayStyle
    displayOnlyMyIssues: boolean
    displayCompletedIssues: boolean
  }
}

export const initialUiState: UiState = {
  issueList: {
    displayStyle: eIssueDisplayStyle.FULL_DETAILS,
    displayOnlyMyIssues: false,
    displayCompletedIssues: true
  },
}

export const getUiState = createFeatureSelector<UiState>( 'ui' )

export const getIssueDisplayStyle = ( state: UiState ) => state.issueList.displayStyle
export const getDisplayOnlyMyIssues = ( state: UiState ) => state.issueList.displayOnlyMyIssues
export const getDisplayCompletedIssues = ( state: UiState ) => state.issueList.displayCompletedIssues
