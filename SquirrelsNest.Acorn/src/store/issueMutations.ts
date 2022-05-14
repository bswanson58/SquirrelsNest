import {request} from 'graphql-request'
import {urlGraphQl} from '../config/endpoints'
import {
  AddIssueInput,
  EditIssueInput,
  Mutation,
  MutationAddIssueArgs,
  MutationEditIssueArgs
} from '../data/graphQlTypes'
import {AddIssueMutation, EditIssueMutation} from '../data/mutationStatements'
import {selectAuthHeader} from './auth'
import {AppThunk} from './configureStore'
import {issueAdded, issueMutationFailed, issueMutationStarted, issueUpdated} from './issues'

export function addIssue( issue: AddIssueInput ): AppThunk {
  return async ( dispatch, getState ) => {
    dispatch( issueMutationStarted() )

    try {
      const authHeader = selectAuthHeader( getState() )
      const variables: MutationAddIssueArgs = {
        issueInput: issue
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

export function editIssue( issue: EditIssueInput ): AppThunk {
  return async ( dispatch, getState ) => {
    dispatch( issueMutationStarted() )

    try {
      const authHeader = selectAuthHeader( getState() )
      const variables: MutationEditIssueArgs = {
        issue: issue
      }

      const data = await request<Mutation>( urlGraphQl, EditIssueMutation, variables, authHeader )

      if( data.editIssue.errors.length > 0 ) {
        dispatch( issueMutationFailed() )
      }
      else if( data.editIssue.issue !== null ) {
        dispatch( issueUpdated( data.editIssue.issue! ) )
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
