import {IssueData, noIssues} from './issueData'
import {useContext, createContext, useState, useEffect} from 'react'
import {APIError, UseClientRequestResult, useManualQuery} from 'graphql-hooks'
import {ISSUES_FOR_PROJECT_QUERY} from './GraphQlQueries'
import {AllIssuesForProjectQueryResult, ClIssue} from './GraphQlEntities'
import {useUserContext} from '../security/UserContext'
import {useProjectContext} from './ProjectContext'
import {noUser} from '../security/user'

interface IIssueContext {
  issueData: IssueData
  loadingErrors: APIError | undefined

  updateIssue( issue: ClIssue ): void
}

const initialContext: IIssueContext = {
  issueData: noIssues,
  loadingErrors: undefined,
  updateIssue() {
  }
}

const IssueContext = createContext<IIssueContext>( initialContext )

function IssueContextProvider( props: any ) {
  const { user } = useUserContext()
  const { currentProject } = useProjectContext()
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
    <IssueContext.Provider
      value={{ issueData: issueData, loadingErrors: loadingErrors, updateIssue: updateIssue }}>
      {props.children}
    </IssueContext.Provider>
  )
}

const useIssueContext = () => useContext( IssueContext )

export {IssueContextProvider, useIssueContext}
