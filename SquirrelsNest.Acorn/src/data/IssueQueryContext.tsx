import {IssueData, noIssues} from './issueData'
import {useContext, createContext, useState, useEffect} from 'react'
import {APIError, UseClientRequestResult, useManualQuery} from 'graphql-hooks'
import {ISSUES_FOR_PROJECT_QUERY} from './GraphQlQueries'
import {AllIssuesForProjectQueryResult, ClIssue} from './GraphQlEntities'
import {useProjectQueryContext} from './ProjectQueryContext'

interface IIssueQueryContext {
  issueData: IssueData
  loadingErrors: APIError | undefined

  requestInitialIssues(): void

  requestAdditionalIssues(): void

  updateIssue( issue: ClIssue ): void
}

const initialContext: IIssueQueryContext = {
  issueData: noIssues,
  loadingErrors: undefined,
  requestInitialIssues() {},
  requestAdditionalIssues() {},
  updateIssue() {}
}

const issuePageSize = 5
const IssueQueryContext = createContext<IIssueQueryContext>( initialContext )

function IssueQueryContextProvider( props: any ) {
  const { currentProject } = useProjectQueryContext()
  const [issueData, setIssueData] = useState<IssueData>( noIssues )
  const [loadingErrors, setLoadingErrors] = useState<APIError>()
  const [skipCount, setSkipCount] = useState(0)

  const [requestIssues, queryResult] = useManualQuery<AllIssuesForProjectQueryResult>(
    ISSUES_FOR_PROJECT_QUERY,
    {
      variables: {
        skip: skipCount,
        take: issuePageSize,
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
    processResponse( queryResult )
  }, [queryResult] )

  async function requestInitialIssues() {
    setLoadingErrors( undefined )
    setIssueData( noIssues )
    setSkipCount(0)

    await requestIssues()
  }

  function requestAdditionalIssues() {

  }

  function updateIssue( newIssue: ClIssue ) {
    issueData.issues = issueData.issues.map( i => i.id === newIssue.id ? newIssue : i )

    setIssueData( issueData )
  }

  return (
    <IssueQueryContext.Provider
      value={{ issueData, loadingErrors, requestInitialIssues, requestAdditionalIssues, updateIssue }}>
      {props.children}
    </IssueQueryContext.Provider>
  )
}

const useIssueQueryContext = () => useContext( IssueQueryContext )

export {IssueQueryContextProvider, useIssueQueryContext}
