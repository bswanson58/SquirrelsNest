import {request} from 'graphql-request'
import {urlGraphQl} from '../config/endpoints'
import {IssuesQuery} from '../data/queryStatements'
import {Query, QueryAllIssuesForProjectArgs} from '../data/graphQlTypes'
import {selectAuthHeader} from './auth'
import {AppThunk} from './configureStore'
import {issueListFailed, issueListPrepare, issueListReceived, issueListRequested} from './issues'

function requestIssues(): AppThunk {
  return async ( dispatch, getState ) => {
    const currentProject = getState().entities.projects.currentProject
    const listState = getState().entities.issues.listState

    if( currentProject === null ) {
      return
    }
    else {
      try {
        const variables: QueryAllIssuesForProjectArgs = {
          skip: listState.skip,
          take: listState.take,
          projectId: currentProject.id,
          order: [
            // @ts-ignore - for an unknown reason, importing SortEnumType causes a module error
            { isFinalized: 'ASC' },
            // @ts-ignore
            { issueNumber: 'DESC' },
          ],
          where: null
        }

        const authHeader = selectAuthHeader( getState() )
        const data = await request<Query>( urlGraphQl, IssuesQuery, variables, authHeader )

        if( data.allIssuesForProject?.items !== undefined ) {
          dispatch( issueListReceived( data.allIssuesForProject! ) )
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

export function requestInitialIssues(): AppThunk {
  return ( dispatch ) => {
    dispatch( issueListPrepare() )
    dispatch( issueListRequested() )
    dispatch( requestIssues() )
  }
}

export function requestAdditionalIssues(): AppThunk {
  return ( dispatch ) => {
    dispatch( issueListRequested() )
    dispatch( requestIssues() )
  }
}
