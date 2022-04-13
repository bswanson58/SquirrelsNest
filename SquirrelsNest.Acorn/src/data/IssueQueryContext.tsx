import {useContext, createContext, useState, useEffect} from 'react'
import {APIError, UseClientRequestResult, useManualQuery} from 'graphql-hooks'
import {noUser} from '../security/user'
import {useUserContext} from '../security/UserContext'
import {ISSUES_FOR_PROJECT_QUERY} from './GraphQlQueries'
import {AllIssuesForProjectQueryResult, ClIssue} from './GraphQlEntities'
import {useProjectQueryContext} from './ProjectQueryContext'

interface IIssueQueryContext {
  issueList: ClIssue[]
  totalIssueCount: number
  loadingErrors: APIError | undefined

  requestInitialIssues(): void

  requestAdditionalIssues(): void

  updateIssue( issue: ClIssue ): void
}

const initialContext: IIssueQueryContext = {
  issueList: [],
  totalIssueCount: 0,
  loadingErrors: undefined,
  requestInitialIssues() {
  },
  requestAdditionalIssues() {
  },
  updateIssue() {
  }
}

const issuePageSize = 5
const IssueQueryContext = createContext<IIssueQueryContext>( initialContext )

function IssueQueryContextProvider( props: any ) {
  const userContext = useUserContext()
  const { currentProject } = useProjectQueryContext()
  const [issueList, setIssueList] = useState<ClIssue[]>( [] )
  const [loadingErrors, setLoadingErrors] = useState<APIError>()
  const [totalIssueCount, setTotalIssueCount] = useState( 0 )
  const [requestEvent, setRequestEvent] = useState( 0 )

  function createQueryVariables() {
    return ({
        variables: {
          skip: issueList.length,
          take: issuePageSize,
          projectId: currentProject?.id
        }
      }
    )
  }

  const [requestIssues, queryResult] = useManualQuery<AllIssuesForProjectQueryResult>( ISSUES_FOR_PROJECT_QUERY )

  const processResponse = ( queryResult: UseClientRequestResult<AllIssuesForProjectQueryResult> ) => {
    const { loading, error, data } = queryResult

    if( loading ) {
      return
    }

    if( error ) {
      console.log( error )

      setLoadingErrors( error )
      setTotalIssueCount( 0 )
      setIssueList( [] )

      return
    }

    if( data ) {
      console.log( `loaded issue data: ${data.allIssuesForProject.totalCount} issues` )

      setLoadingErrors( undefined )
      setIssueList( [...issueList, ...data.allIssuesForProject.items] )
      setTotalIssueCount( data.allIssuesForProject.totalCount )
    }
  }

  useEffect( () => {
    processResponse( queryResult )
  }, [queryResult] )

  useEffect( () => {
    if( (userContext.user !== noUser) &&
      (currentProject !== undefined) ) {
      setLoadingErrors( undefined )
      setTotalIssueCount( 0 )
      setIssueList( [] )
      setRequestEvent( requestEvent + 1 )
    }
  }, [userContext, currentProject] )

  useEffect( () => {
    (async () => requestInitialIssues())()
  }, [requestEvent] )

  async function requestInitialIssues() {
    if( (userContext.user !== noUser) &&
      (currentProject !== undefined) ) {
      await requestIssues( createQueryVariables() )
    }
  }

  async function requestAdditionalIssues() {
    setLoadingErrors( undefined )

    await requestIssues( createQueryVariables() )
  }

  function updateIssue( newIssue: ClIssue ) {
    setIssueList( issueList.map( i => i.id === newIssue.id ? newIssue : i ) )
  }

  return (
    <IssueQueryContext.Provider
      value={{ issueList, totalIssueCount, loadingErrors, requestInitialIssues, requestAdditionalIssues, updateIssue }}>
      {props.children}
    </IssueQueryContext.Provider>
  )
}

const useIssueQueryContext = () => useContext( IssueQueryContext )

export {IssueQueryContextProvider, useIssueQueryContext}
