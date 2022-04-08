import {UseClientRequestResult, useManualQuery} from 'graphql-hooks'
import {createContext, useContext, useEffect, useState} from 'react'
import {ADD_ISSUE_MUTATION} from './graphQlMutations'
import {useIssueQueryContext} from './IssueQueryContext'
import {AddIssueInput, AddIssuePayload} from './mutationEntities'

interface IIssueMutationContext {
  addIssue( newIssue: AddIssueInput ): void
}

const initialContext: IIssueMutationContext = {
  addIssue() {
  }
}

const IssueMutationContext = createContext<IIssueMutationContext>( initialContext )

function IssueMutationContextProvider( props: any ) {
  const [issueInput, setIssueInput] = useState<AddIssueInput>( { title: '', description: '', projectId: '' } )
  const currentIssues = useIssueQueryContext()

  const [requestAddIssue, mutationResult] = useManualQuery<AddIssuePayload>(
    ADD_ISSUE_MUTATION,
    {
      variables: {
        'issue': {
          'projectId': issueInput.projectId,
          'title': issueInput.title,
          'description': issueInput.description
        }
      },
    }
  )

  const processData = ( data: AddIssuePayload ) => {
    console.log( `Added issue: ${data.addIssue.issue.title}` )

    if( data.addIssue.errors.length > 0 ) {
      console.log( `Error adding issue: ${data.addIssue.errors[0]}` )
    } else {
      currentIssues.updateIssue( data.addIssue.issue )
    }
  }

  const processResponse = ( queryResult: UseClientRequestResult<AddIssuePayload> ) => {
    const { loading, error, data } = queryResult

    if( loading ) {
      return
    }

    if( error ) {
      console.log( error )

      return
    }

    if( data ) {
      processData( data )
    }
  }

  useEffect( () => {
    if( issueInput?.projectId.length > 0 ) {
      (async () => requestAddIssue())()
    }
  }, [issueInput, requestAddIssue] )

  useEffect( () => {
    processResponse( mutationResult )
  }, [mutationResult] )

  const addIssue = ( newIssue: AddIssueInput ) => {
    setIssueInput( newIssue )
  }

  return (
    <IssueMutationContext.Provider
      value={{ addIssue: addIssue }}>
      {props.children}
    </IssueMutationContext.Provider>
  )

}

const useIssueMutationContext = () => useContext( IssueMutationContext )

export {IssueMutationContextProvider, useIssueMutationContext}
