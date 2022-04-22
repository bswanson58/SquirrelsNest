import {request} from 'graphql-request'
import {urlGraphQl} from '../config/endpoints'
import {AllProjectsQuery} from '../data/queryStatements'
import {ClProject, Query, QueryProjectListArgs} from '../data/graphQlTypes'
import {selectAuthHeader} from './auth'
import {AppThunk} from './configureStore'
import {requestInitialIssues} from './issueActions'
import {projectListFailed, projectListReceived, projectListRequested, projectSetCurrent} from './projects'

export function requestProjectList(/* args: QueryAllProjectsArgs */ ): AppThunk {
  return async ( dispatch, getState ) => {
    dispatch( projectListRequested() )

    try {
      const variables: QueryProjectListArgs = {
        first: 10,
      }

      const authHeader = selectAuthHeader( getState() )
      const data = await request<Query>( urlGraphQl, AllProjectsQuery, variables, authHeader )

      if( data.projectList !== undefined ) {
        dispatch( projectListReceived( data.projectList! ) )
      }
    }
    catch( error: any ) {
      if( error?.response?.errors?.length !== undefined ) {
        dispatch( projectListFailed( error.response.errors[0].message ) )
      }
      else if( error.response.error !== undefined ) {
        dispatch( projectListFailed( `(${error.response.status}) ${error.response.error}` ) )
      }
      else {
        dispatch( projectListFailed( '' ) )
      }
//      console.error( JSON.stringify( error, undefined, 2 ) )
    }
  }
}

export function setCurrentProject( project: ClProject ): AppThunk {
  return ( dispatch ) => {
    dispatch( projectSetCurrent( project ) )
    dispatch( requestInitialIssues() )
  }
}
