import {createSlice, PayloadAction} from '@reduxjs/toolkit'
import {LoginPayload} from '../data/graphQlTypes'
import {claim} from '../security/authenticationModels'
import {userEmail, userName} from '../security/userClaims'
import {RootState} from './configureStore'
import {getAuthenticationClaims, saveAuthenticationToken} from '../security/jwtSupport'

interface AuthState {
  userClaims: claim[]
  token: String
  expiration: Number
  loading: boolean
}

const initialState: AuthState = {
  userClaims: [],
  token: '',
  expiration: 0,
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

    authReceived: ( authState, action: PayloadAction<LoginPayload> ) => {
      authState.token = action.payload.token
      authState.expiration = action.payload.expiration
      authState.loading = false

      saveAuthenticationToken( action.payload )
      authState.userClaims = getAuthenticationClaims()

      console.log( `auth received: ${JSON.stringify( action.payload, undefined, 2 )}` )
    },

    authFailed: ( authState, action: PayloadAction<string> ) => {
      authState.loading = false

      console.log( `auth received: ${JSON.stringify( action.payload, undefined, 2 )}` )
    },

    authLoaded: ( authState, action: PayloadAction<{token: string, expiration: number}> ) => {
      authState.token = action.payload.token
      authState.expiration = action.payload.expiration

      authState.userClaims = getAuthenticationClaims()

      console.log( `authentication loaded: ${userName( authState.userClaims )}`)
    },
  }
} )

export function selectIsUserAuthenticated( state: RootState ): boolean {
  return state.auth.token.length > 0
}

export function selectIsAuthenticating( state: RootState ): boolean {
  return state.auth.loading
}

export function selectUserClaims( state: RootState ): claim[] {
  return state.auth.userClaims
}

export function selectUserName( state: RootState ): string {
  const claims = state.auth.userClaims

  return userName( claims )
}

export function selectUserEmail( state: RootState ): string {
  const claims = state.auth.userClaims

  return userEmail( claims )
}

export function selectAuthHeader( state: RootState ): HeadersInit {
  return ({
    'authorization': `bearer ${state.auth.token}`
  })
}

export const {
  authRequested,
  authReceived,
  authFailed,
  authLoaded,
} = slice.actions

export default slice.reducer
