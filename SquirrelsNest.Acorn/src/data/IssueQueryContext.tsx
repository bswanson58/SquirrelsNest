import {IssueData, noIssues} from './issueData'
import {useContext, createContext, useState, useEffect} from 'react'
import {APIError, UseClientRequestResult, useManualQuery} from 'graphql-hooks'
import {ISSUES_FOR_PROJECT_QUERY} from './GraphQlQueries'
import {AllIssuesForProjectQueryResult, ClIssue} from './GraphQlEntities'
import {useUserContext} from '../security/UserContext'
import {useProjectQueryContext} from './ProjectQueryContext'
import {noUser} from '../security/user'

interface IIssueQueryContext {
  issueData: IssueData
  loadingErrors: APIError | undefined

  updateIssue( issue: ClIssue ): void
}

const initialContext: IIssueQueryContext = {
  issueData: noIssues,
  loadingErrors: undefined,
  updateIssue() {
  }
}

const IssueQueryContext = createContext<IIssueQueryContext>( initialContext )

function IssueQueryContextProvider( props: any ) {
  const { user } = useUserContext()
  const { currentProject } = useProjectQueryContext()
  const [issueData, setIssueData] = useState<IssueData>( noIssues )
  const [loadingErrors, setLoadingErrors] = useState<APIError>()

  const [requestIssues, queryResult] = useManualQuery<AllIssuesForProjectQueryResult>(
    ISSUES_FOR_PROJECT_QUERY,
    {
      variables: {
        first: 5,
        projectId: currentProject?.id
      },
    }
  )

  const processResponse = ( queryResult: UseClientRequestResult<AllIssuesForProjectQueryResult> ) => {
    const { loading, error, data } = queryResult

    if( loading ) {
      return
    }

    if( error ) {
      console.log( error )

      setLoadingErrors( error )
      setIssueData( noIssues )

      return
    }

    if( data ) {
      console.log( `loaded issue data: ${data.allIssuesForProject.totalCount} issues` )

      setLoadingErrors( undefined )
      setIssueData( new IssueData( data ) )
    }
  }

  useEffect( () => {
    setLoadingErrors( undefined )
    setIssueData( noIssues )

    if( (user !== noUser) &&
      (currentProject !== undefined) ) {
      (async () => requestIssues())()
    }
  }, [user, currentProject, requestIssues] )

  useEffect( () => {
    processResponse( queryResult )
  }, [queryResult] )

  function updateIssue( newIssue: ClIssue ) {
    issueData.issues = issueData.issues.map( i => i.id === newIssue.id ? newIssue : i )

    setIssueData( issueData )
  }

  return (
    <IssueQueryContext.Provider
      value={{ issueData: issueData, loadingErrors: loadingErrors, updateIssue: updateIssue }}>
      {props.children}
    </IssueQueryContext.Provider>
  )
}

const useIssueQueryContext = () => useContext( IssueQueryContext )

export {IssueQueryContextProvider, useIssueQueryContext}
