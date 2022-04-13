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
  requestInitialIssues() {},
  requestAdditionalIssues() {},
  updateIssue() {}
}

const issuePageSize = 5
const IssueQueryContext = createContext<IIssueQueryContext>( initialContext )

function IssueQueryContextProvider( props: any ) {
  const userContext = useUserContext()
  const { currentProject } = useProjectQueryContext()
  const [issueList, setIssueList] = useState<ClIssue[]>( [] )
  const [loadingErrors, setLoadingErrors] = useState<APIError>()
  const [totalIssueCount, setTotalIssueCount] = useState( 0 )

  const [queryIssues] = useManualQuery<AllIssuesForProjectQueryResult>( ISSUES_FOR_PROJECT_QUERY )

  function createQueryVariables( skipCount: number ) {
    return ({
      variables: {
        skip: skipCount,
        take: issuePageSize,
        projectId: currentProject?.id,
        order: {
          isFinalized: 'ASC',
          issueNumber: 'DESC',
        },
      }
    })
  }

  function clearIssues() {
    setTotalIssueCount( 0 )
    setIssueList( [] )
  }

  function processResponse( queryResult: UseClientRequestResult<AllIssuesForProjectQueryResult>, replaceCurrentIssues: boolean ) {
    const { loading, error, data } = queryResult

    if( loading ) {
      return
    }

    if( error ) {
      console.log( error )

      clearIssues()
      setLoadingErrors( error )

      return
    }

    if( data ) {
      console.log( `loaded issue data: ${data.allIssuesForProject.totalCount} issues` )

      setLoadingErrors( undefined )
      setTotalIssueCount( data.allIssuesForProject.totalCount )
      setIssueList( replaceCurrentIssues ?
        data.allIssuesForProject.items :
        [...issueList, ...data.allIssuesForProject.items] )
    }
  }

  function requestIssues( isInitialRequest: boolean ) {
    if( (userContext.user !== noUser) &&
      (currentProject !== undefined) ) {
      queryIssues( createQueryVariables( isInitialRequest ? 0 : issueList.length ) )
        .then( data => processResponse( data, isInitialRequest ) )
        .catch( reason => {
          // setLoadingErrors(...)
          console.log( reason )
        } )
    }
  }

  function requestInitialIssues() {
    clearIssues()

    requestIssues( true )
  }

  function requestAdditionalIssues() {
    requestIssues( false )
  }

  function updateIssue( newIssue: ClIssue ) {
    setIssueList( issueList.map( i => i.id === newIssue.id ? newIssue : i ) )
  }

  useEffect( () => {
    if( (userContext.user !== noUser) &&
      (currentProject !== undefined) ) {
      requestInitialIssues()
    }
  }, [userContext, currentProject] )

  return (
    <IssueQueryContext.Provider
      value={{ issueList, totalIssueCount, loadingErrors, requestInitialIssues, requestAdditionalIssues, updateIssue }}>
      {props.children}
    </IssueQueryContext.Provider>
  )
}

const useIssueQueryContext = () => useContext( IssueQueryContext )

export {IssueQueryContextProvider, useIssueQueryContext}
