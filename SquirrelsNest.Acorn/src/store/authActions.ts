import {request} from 'graphql-request'
import {urlGraphQl} from '../config/endpoints'
import {LoginMutation} from '../data/mutationStatements'
import {Mutation, MutationLoginArgs} from '../data/graphQlTypes'
import {userCredentials} from '../security/authenticationModels'
import {authFailed, authReceived, authRequested} from './auth'
import {AppThunk} from './configureStore'
import {requestInitialProjects} from './projectActions'

export function loginUser( credentials: userCredentials ): AppThunk {
  return async ( dispatch /*, getState */ ) => {
    dispatch( authRequested() )

    try {
      const variables: MutationLoginArgs = {
        loginInput: {
          email: credentials.email,
          password: credentials.password,
        }
      }

      const data = await request<Mutation>( urlGraphQl, LoginMutation, variables )

      dispatch( authReceived( data.login ) )
      dispatch( requestInitialProjects() )
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
