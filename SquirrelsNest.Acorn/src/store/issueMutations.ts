import {request} from 'graphql-request'
import {urlGraphQl} from '../config/endpoints'
import {AddIssueInput, Mutation, MutationAddIssueArgs} from '../data/graphQlTypes'
import {AddIssueMutation} from '../data/mutationStatements'
import {selectAuthHeader} from './auth'
import {AppThunk} from './configureStore'
import {issueAdded, issueMutationFailed, issueMutationStarted} from './issues'

export function addIssue( issue: AddIssueInput ): AppThunk {
  return async ( dispatch, getState ) => {
    dispatch( issueMutationStarted() )

    try {
      const authHeader = selectAuthHeader( getState() )
      const variables: MutationAddIssueArgs = {
        issue: issue
      }

      const data = await request<Mutation>( urlGraphQl, AddIssueMutation, variables, authHeader )

      if( data.addIssue.errors.length > 0 ) {
        dispatch( issueMutationFailed() )
      }
      else if( data.addIssue.issue !== null ) {
        dispatch( issueAdded( data.addIssue.issue! ) )
      }
      else {
        dispatch( issueMutationFailed() )
      }
    }
    catch( error: any ) {
      dispatch( issueMutationFailed() )
    }
  }
}
