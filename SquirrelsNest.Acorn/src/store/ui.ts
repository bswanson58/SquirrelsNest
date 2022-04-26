import {createSlice, PayloadAction} from '@reduxjs/toolkit'
import {RootState} from './configureStore'
import {ModalPayload, ModalProperties} from '../components/ModalRoot'

export enum eDisplayStyle { TITLE_ONLY, TITLE_DESCRIPTION, FULL_DETAILS }

interface uiState {
  issueList: {
    displayStyle: eDisplayStyle
  }
  modals: {
    payload: ModalPayload
  }
}

const initialState: uiState = {
  issueList: {
    displayStyle: eDisplayStyle.TITLE_DESCRIPTION
  },
  modals: {
    payload: {
      modalType: '',
      modalState: false,
      modalProps: {}
    }
  }
}

const slice = createSlice( {
  name: 'ui',
  initialState: initialState,
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
    },

    modalShow: ( uiState, action: PayloadAction<ModalProperties> ) => {
      uiState.modals.payload.modalType = action.payload.modalType
      uiState.modals.payload.modalProps = action.payload.modalProps
      uiState.modals.payload.modalState = true
    },

    modalHide: ( uiState ) => {
      uiState.modals.payload.modalType = ''
      uiState.modals.payload.modalProps = {}
      uiState.modals.payload.modalState = false
    },
  }
} )

export function selectIssueListStyle( state: RootState ): eDisplayStyle {
  return state.ui.issueList.displayStyle
}

export const {
  toggleIssueListStyle,
  modalShow,
  modalHide,
} = slice.actions

export default slice.reducer
