import {createSlice, PayloadAction} from '@reduxjs/toolkit'
import {AuthenticationResponse} from '../data/graphQlTypes'

interface AuthState {
  token: String
  expiration: Number
  email: String
  userName: String
  loading: boolean
}

const initialState: AuthState = {
  token: '',
  expiration: 0,
  email: '',
  userName: '',
  loading: false
}

const slice = createSlice( {
  name: 'auth',
  initialState: initialState,
  // actions => actionHandlers
  reducers: {
    authRequested: ( authState ) => {
      authState.loading = true

      console.log( `auth requested` )
    },

    authReceived: ( authState, action: PayloadAction<AuthenticationResponse> ) => {
      authState.token = action.payload.token
      authState.expiration = action.payload.expiration
      authState.email = ''
      authState.userName = ''
      authState.loading = false

      console.log( `auth received: ${JSON.stringify( action.payload, undefined, 2 )}` )
    },

    authFailed: ( authState, action: PayloadAction<string> ) => {
      authState.loading = false

      console.log( `auth received: ${JSON.stringify( action.payload, undefined, 2 )}` )
    },
  }
} )

export const {
  authRequested,
  authReceived,
  authFailed,
} = slice.actions

export default slice.reducer
