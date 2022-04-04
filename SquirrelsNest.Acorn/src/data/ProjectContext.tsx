import { ProjectList, noProjects } from './projectList'
import { useContext, createContext, useState, useEffect } from 'react'
import { APIError, useQuery } from 'graphql-hooks'
import { PROJECTS_QUERY } from '../data/GraphQlQueries'
import { AllProjectsQueryResult, ClProject } from '../data/GraphQlEntities'
import UserContext from '../security/UserContext'
import { noUser } from '../security/user'

const ProjectContext = createContext<{
  projects: ProjectList
  loadingErrors: APIError<object> | undefined
  currentProject: ClProject | undefined
  setCurrentProject(project: ClProject): void
}>({ projects: noProjects, loadingErrors: undefined, currentProject: undefined, setCurrentProject: () => {} })

function ProjectContextProvider(props: any) {
  const { user } = useContext(UserContext)
  const [projectList, setProjectList] = useState<ProjectList>(noProjects)
  const [loadingErrors, setLoadingErrors] = useState<APIError<object>>()
  const [currentProject, setCurrentProject] = useState<ClProject>()

  const queryResult = useQuery<AllProjectsQueryResult>(
    PROJECTS_QUERY,
    {
      variables: {
        limit: 10,
      },
    }
  )

  useEffect(() => {
    const { loading, error, data } = queryResult

    if (user === noUser) {
      setLoadingErrors(undefined)
      setProjectList(noProjects)

      return
    }

    if (loading) {
      return
    }

    if (error) {
      console.log(error)

      setLoadingErrors(error)
      setProjectList(noProjects)

      return
    }

    if (data) {
      console.log(`loaded project data: ${data.allProjects.totalCount} projects`)
      
      setLoadingErrors(undefined)
      setProjectList(new ProjectList(data))
    }
  }, [user, queryResult.loading])

  return (
    <ProjectContext.Provider
      value={{ projects: projectList, loadingErrors: loadingErrors, currentProject: currentProject, setCurrentProject: p => setCurrentProject(p) }}>
      {props.children}
    </ProjectContext.Provider>
  )
}

const useProjectContext = () => useContext(ProjectContext);

export { ProjectContextProvider, useProjectContext }
