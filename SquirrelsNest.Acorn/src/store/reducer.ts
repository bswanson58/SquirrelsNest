import {combineReducers} from 'redux'
import authReducer from './auth'
import issueReducer from './issues'
import projectReducer from './projects'
import uiReducer from './ui'

const entitiesReducer = combineReducers( {
  issues: issueReducer,
  projects: projectReducer
} )

export const appReducer = combineReducers( {
  auth: authReducer,
  entities: entitiesReducer,
  ui: uiReducer,
} )
