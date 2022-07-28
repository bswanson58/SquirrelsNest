import {createFeatureSelector} from '@ngrx/store'

export enum eIssueDisplayStyle { TITLE_ONLY, TITLE_DESCRIPTION, FULL_DETAILS }

export interface UiState {
  issueList: {
    displayStyle: eIssueDisplayStyle
  }
}

export const initialUiState: UiState = {
  issueList: {
    displayStyle: eIssueDisplayStyle.FULL_DETAILS
  },
}

export const getUiState = createFeatureSelector<UiState>( 'ui' )

export const getIssueDisplayStyle = ( state: UiState ) => state.issueList.displayStyle
