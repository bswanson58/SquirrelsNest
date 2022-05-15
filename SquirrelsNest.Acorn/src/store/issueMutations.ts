import {request} from 'graphql-request'
import {urlGraphQl} from '../config/endpoints'
import {
  AddIssueInput,
  EditIssueInput,
  Mutation,
  MutationAddIssueArgs,
  MutationEditIssueArgs, MutationUpdateIssueArgs, UpdateIssueInput
} from '../data/graphQlTypes'
import {AddIssueMutation, EditIssueMutation, UpdateIssueMutation} from '../data/mutationStatements'
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

export function updateIssue( updateInfo: UpdateIssueInput ): AppThunk {
  return async ( dispatch, getState ) => {
    dispatch( issueMutationStarted() )

    try {
      const authHeader = selectAuthHeader( getState() )
      const variables: MutationUpdateIssueArgs = {
        updateInput: updateInfo
      }

      const data = await request<Mutation>( urlGraphQl, UpdateIssueMutation, variables, authHeader )

      if( data.updateIssue.errors.length > 0 ) {
        dispatch( issueMutationFailed() )
      }
      else if( data.updateIssue.issue !== null ) {
        dispatch( issueUpdated( data.updateIssue.issue! ) )
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
