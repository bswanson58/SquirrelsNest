import {AnyAction, configureStore, ThunkAction} from '@reduxjs/toolkit'
import {appReducer} from './reducer'

export const appStore = configureStore( {
    preloadedState: undefined,
    reducer: appReducer
  } )

export type RootState = ReturnType<typeof appStore.getState>
export type AppDispatch = typeof appStore.dispatch
export type AppThunk<ReturnType = void> = ThunkAction<ReturnType, RootState, unknown, AnyAction>
