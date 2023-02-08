import {createFeatureSelector} from '@ngrx/store'

export enum eIssueDisplayStyle { TITLE_ONLY, TITLE_DESCRIPTION, FULL_DETAILS }

export interface UiState {
  issueList: {
    displayStyle: eIssueDisplayStyle
    displayOnlyMyIssues: boolean
    displayCompletedIssues: boolean
  },
  errors: {
    lastError: string
  }
  service: {
    isActive: boolean,
    serviceActivity: string
  }
}

export const initialUiState: UiState = {
  issueList: {
    displayStyle: eIssueDisplayStyle.FULL_DETAILS,
    displayOnlyMyIssues: false,
    displayCompletedIssues: true
  },
  errors: {
    lastError: ''
  },
  service: {
    isActive: false,
    serviceActivity: ''
  }
}

export const getUiState = createFeatureSelector<UiState>( 'ui' )

export const getIssueDisplayStyle = ( state: UiState ) => state.issueList.displayStyle
export const getDisplayOnlyMyIssues = ( state: UiState ) => state.issueList.displayOnlyMyIssues
export const getDisplayCompletedIssues = ( state: UiState ) => state.issueList.displayCompletedIssues
export const getLastError = ( state: UiState ) => state.errors.lastError
export const getServiceIsActive = ( state: UiState ) => state.service.isActive
export const getServiceActivity = ( state: UiState ) => state.service.serviceActivity
