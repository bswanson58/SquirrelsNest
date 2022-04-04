import { IssueData, noIssues } from './issueData'
import { useContext, createContext, useState, useEffect } from 'react'
import { APIError, UseClientRequestResult, useManualQuery } from 'graphql-hooks'
import { ISSUES_FOR_PROJECT_QUERY } from '../data/GraphQlQueries'
import { AllIssuesForProjectQueryResult } from '../data/GraphQlEntities'
import { useUserContext } from '../security/UserContext'
import { useProjectContext } from './ProjectContext'
import { noUser } from '../security/user'

const IssueContext = createContext<{
  issueData: IssueData
  loadingErrors: APIError<object> | undefined
}>({ issueData: noIssues, loadingErrors: undefined })

function IssueContextProvider(props: any) {
  const { user } = useUserContext()
  const { currentProject } = useProjectContext()
  const [issueData, setIssueData] = useState<IssueData>(noIssues)
  const [loadingErrors, setLoadingErrors] = useState<APIError<object>>()

  const [ requestIssues, queryResult ] = useManualQuery<AllIssuesForProjectQueryResult>(
    ISSUES_FOR_PROJECT_QUERY,
    {
      variables: {
        first: 5,
        projectId: "05be38d6-2751-49bf-8a48-4c1823a69f7d"
      },
    }
  )

  const processResponse = ( queryResult: UseClientRequestResult<AllIssuesForProjectQueryResult> ) => {
    const { loading, error, data } = queryResult

    if (loading) {
      return
    }

    if (error) {
      console.log(error)

      setLoadingErrors(error)
      setIssueData(noIssues)

      return
    }

    if (data) {
      console.log(`loaded issue data: ${data.allIssuesForProject.totalCount} issues`)
      
      setLoadingErrors(undefined)
      setIssueData(new IssueData(data))
    }
  }

  useEffect(() => {
    setLoadingErrors(undefined)
    setIssueData(noIssues)

    if (user !== noUser) {
      requestIssues()

      console.log('requested issue data')
    }
  }, [user, currentProject])

  useEffect(() => {
    processResponse(queryResult)
  }, [queryResult.data, queryResult.error])

  return (
    <IssueContext.Provider
      value={{ issueData: issueData, loadingErrors: loadingErrors }}>
      {props.children}
    </IssueContext.Provider>
  )
}

const useIssueContext = () => useContext(IssueContext);

export { IssueContextProvider, useIssueContext }
