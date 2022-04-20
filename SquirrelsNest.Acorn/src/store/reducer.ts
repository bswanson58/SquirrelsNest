import {combineReducers} from 'redux'
import authReducer from './auth'
import issueReducer from './issues'
import projectReducer from './projects'

const entitiesReducer = combineReducers( {
  auth: authReducer,
  issues: issueReducer,
  projects: projectReducer
} )

export const appReducer = combineReducers( {
  entities: entitiesReducer
} )
