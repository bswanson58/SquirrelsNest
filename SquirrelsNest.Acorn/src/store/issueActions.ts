import {request} from 'graphql-request'
import {urlGraphQl} from '../config/endpoints'
import {IssuesQuery} from '../data/GraphQlQueries'
import {Query, QueryAllIssuesForProjectArgs} from '../data/graphQlTypes'
import {selectAuthHeader} from './auth'
import {AppThunk} from './configureStore'
import {issueListFailed, issueListReceived, issueListRequested} from './issues'

export function requestIssueList(/* args: QueryAllProjectsArgs */ ): AppThunk {
  return async ( dispatch, getState ) => {
    const currentProject = getState().entities.projects.currentProject

    if( currentProject === null ) {
      return
    }
    else {
      dispatch( issueListRequested() )

      try {
        const variables: QueryAllIssuesForProjectArgs = {
          skip: 0,
          take: 3,
          projectId: currentProject.id,
          order: [],
          where: null
        }

        const authHeader = selectAuthHeader( getState() )
        const data = await request<Query>( urlGraphQl, IssuesQuery, variables, authHeader )

        if( data.allIssuesForProject?.items !== undefined ) {
          dispatch( issueListReceived( data.allIssuesForProject.items! ) )
        }
      }
      catch( error: any ) {
        if( error?.response?.errors?.length !== undefined ) {
          dispatch( issueListFailed( error.response.errors[0].message ) )
        }
        else if( error.response.error !== undefined ) {
          dispatch( issueListFailed( `(${error.response.status}) ${error.response.error}` ) )
        }
        else {
          dispatch( issueListFailed( '' ) )
        }
//      console.error( JSON.stringify( error, undefined, 2 ) )
      }
    }
  }
}

export function requestAdditionalIssues(): AppThunk {
  return async ( dispatch, getState ) => {
  }
}
