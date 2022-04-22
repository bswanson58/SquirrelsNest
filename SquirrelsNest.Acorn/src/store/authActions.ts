import {request} from 'graphql-request'
import {urlGraphQl} from '../config/endpoints'
import {LoginQuery} from '../data/queryStatements'
import {Query, QueryLoginArgs} from '../data/graphQlTypes'
import {userCredentials} from '../security/authenticationModels'
import {authFailed, authReceived, authRequested} from './auth'
import {AppThunk} from './configureStore'
import {requestProjectList} from './projectActions'

export function loginUser( credentials: userCredentials ): AppThunk {
  return async ( dispatch /*, getState */ ) => {
    dispatch( authRequested() )

    try {
      const variables: QueryLoginArgs = {
        userCredentials: {
          email: credentials.email,
          password: credentials.password,
          name: ''
        }
      }

      const data = await request<Query>( urlGraphQl, LoginQuery, variables )

      dispatch( authReceived( data.login ) )
      dispatch( requestProjectList() )
    }
    catch( error: any ) {
      if( error?.response?.errors?.length !== undefined ) {
        dispatch( authFailed( error.response.errors[0].message ) )
      }
      else if( error.response.error !== undefined ) {
        dispatch( authFailed( `(${error.response.status}) ${error.response.error}` ) )
      }
      else {
        dispatch( authFailed( '' ) )
      }

//      console.error( JSON.stringify( error, undefined, 2 ) )
    }
  }
}
