import {createFeatureSelector} from '@ngrx/store'
import {claim} from './auth.models'

export interface AuthState {
  userClaims: claim[]
  token: String
  expiration: Number
  loading: boolean
}

export const initialAuthState: AuthState = {
  userClaims: [],
  token: '',
  expiration: 0,
  loading: false
}

export const getAuthState = createFeatureSelector<AuthState>( 'auth' )

export const getIsLoading = ( state: AuthState ) => state.loading
