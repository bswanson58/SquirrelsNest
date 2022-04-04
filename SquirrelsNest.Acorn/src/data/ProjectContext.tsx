import { ProjectData, noProjects } from './projectData'
import { useContext, createContext, useState, useEffect } from 'react'
import { APIError, UseClientRequestResult, useManualQuery } from 'graphql-hooks'
import { PROJECTS_QUERY } from '../data/GraphQlQueries'
import { AllProjectsQueryResult, ClProject } from '../data/GraphQlEntities'
import { useUserContext } from '../security/UserContext'
import { noUser } from '../security/user'

const ProjectContext = createContext<{
  projects: ProjectData
  loadingErrors: APIError<object> | undefined
  currentProject: ClProject | undefined
  setCurrentProject(project: ClProject): void
}>({ projects: noProjects, loadingErrors: undefined, currentProject: undefined, setCurrentProject: () => {} })

function ProjectContextProvider(props: any) {
  const { user } = useUserContext()
  const [projectData, setProjectData] = useState<ProjectData>(noProjects)
  const [loadingErrors, setLoadingErrors] = useState<APIError<object>>()
  const [currentProject, setCurrentProject] = useState<ClProject>()

  const [ requestProjects, queryResult ] = useManualQuery<AllProjectsQueryResult>(
    PROJECTS_QUERY,
    {
      variables: {
        limit: 10,
      },
    }
  )

  const processResponse = ( queryResult: UseClientRequestResult<AllProjectsQueryResult> ) => {
    const { loading, error, data } = queryResult

    if (loading) {
      return
    }

    if (error) {
      console.log(error)

      setLoadingErrors(error)
      setProjectData(noProjects)

      return
    }

    if (data) {
      console.log(`loaded project data: ${data.allProjects.totalCount} projects`)
      
      setLoadingErrors(undefined)
      setProjectData(new ProjectData(data))
    }
  }

  useEffect(() => {
    setLoadingErrors(undefined)
    setProjectData(noProjects)

    if (user !== noUser) {
      requestProjects()

      console.log('requested project data')
    }
  }, [user])

  useEffect(() => {
    processResponse(queryResult)
  }, [queryResult.data, queryResult.error])

  return (
    <ProjectContext.Provider
      value={{ projects: projectData, loadingErrors: loadingErrors, currentProject: currentProject, setCurrentProject: p => setCurrentProject(p) }}>
      {props.children}
    </ProjectContext.Provider>
  )
}

const useProjectContext = () => useContext(ProjectContext);

export { ProjectContextProvider, useProjectContext }
