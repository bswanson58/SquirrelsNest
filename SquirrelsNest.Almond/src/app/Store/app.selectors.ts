import {createSelector} from '@ngrx/store'
import * as fromAuth from '../Auth/auth.state'

export const getIsAuthLoading = createSelector( fromAuth.getAuthState, fromAuth.getIsLoading )
export const getIsAuthenticated = createSelector( fromAuth.getAuthState, fromAuth.getIsAuthenticated )
