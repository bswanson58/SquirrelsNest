import {createSlice} from '@reduxjs/toolkit'
import {RootState} from './configureStore'

export enum eDisplayStyle { TITLE_ONLY, TITLE_DESCRIPTION, FULL_DETAILS }

interface uiState {
  issueList: {
    displayStyle: eDisplayStyle
  }
}

const initialState: uiState = {
  issueList: {
    displayStyle: eDisplayStyle.TITLE_DESCRIPTION
  }
}

const slice = createSlice( {
  name: 'ui',
  initialState: initialState,
  // actions => actionHandlers
  reducers: {
    toggleIssueListStyle: ( uiState ) => {
      console.log( 'toggle issue list style' )
      switch( uiState.issueList.displayStyle ) {
        case eDisplayStyle.FULL_DETAILS:
          uiState.issueList.displayStyle = eDisplayStyle.TITLE_DESCRIPTION
          break
        case eDisplayStyle.TITLE_DESCRIPTION:
          uiState.issueList.displayStyle = eDisplayStyle.TITLE_ONLY
          break
        case eDisplayStyle.TITLE_ONLY:
          uiState.issueList.displayStyle = eDisplayStyle.FULL_DETAILS
          break
      }
    }
  }
} )

export function selectIssueListStyle( state: RootState ): eDisplayStyle {
  return state.ui.issueList.displayStyle
}

export const {
  toggleIssueListStyle,
} = slice.actions

export default slice.reducer
