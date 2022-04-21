import {createSlice, PayloadAction} from '@reduxjs/toolkit'
import {AuthenticationResponse} from '../data/graphQlTypes'
import {RootState} from './configureStore'
import {getAuthenticationClaims, saveAuthenticationToken} from '../security/jwtSupport'
import {User, noUser} from '../security/user'

interface AuthState {
  user: User
  token: String
  expiration: Number
  loading: boolean
}

const initialState: AuthState = {
  user: noUser,
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

    authReceived: ( authState, action: PayloadAction<AuthenticationResponse> ) => {
      authState.token = action.payload.token
      authState.expiration = action.payload.expiration
      authState.loading = false

      saveAuthenticationToken( action.payload )
      authState.user = new User( getAuthenticationClaims())

      console.log( `auth received: ${JSON.stringify( action.payload, undefined, 2 )}` )
    },

    authFailed: ( authState, action: PayloadAction<string> ) => {
      authState.loading = false

      console.log( `auth received: ${JSON.stringify( action.payload, undefined, 2 )}` )
    },
  }
} )

export function selectIsUserAuthenticated( state: RootState ): boolean {
  return state.auth.token.length > 0
}

export function selectIsAuthenticating( state: RootState ) : boolean {
  return state.auth.loading
}

export function selectAuthUser(state: RootState ) : User {
  return state.auth.user
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
} = slice.actions

export default slice.reducer
