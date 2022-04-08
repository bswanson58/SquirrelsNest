import {ProjectData, noProjects} from './projectData'
import {useContext, createContext, useState, useEffect} from 'react'
import {APIError, UseClientRequestResult, useManualQuery} from 'graphql-hooks'
import {PROJECTS_QUERY} from './GraphQlQueries'
import {AllProjectsQueryResult, ClProject} from './GraphQlEntities'
import {useUserContext} from '../security/UserContext'
import {noUser} from '../security/user'

interface IProjectQueryContext {
  projects: ProjectData
  loadingErrors: APIError | undefined
  currentProject: ClProject | undefined

  setCurrentProject( project: ClProject ): void
}

const initialContext: IProjectQueryContext = {
  projects: noProjects,
  loadingErrors: undefined,
  currentProject: undefined,
  setCurrentProject() {
  }
}

const ProjectQueryContext = createContext<IProjectQueryContext>( initialContext )

function ProjectQueryContextProvider( props: any ) {
  const { user } = useUserContext()
  const [projectData, setProjectData] = useState<ProjectData>( noProjects )
  const [loadingErrors, setLoadingErrors] = useState<APIError>()
  const [currentProject, setCurrentProject] = useState<ClProject>()

  const [requestProjects, queryResult] = useManualQuery<AllProjectsQueryResult>(
    PROJECTS_QUERY,
    {
      variables: {
        first: 10,
      },
    }
  )

  const processResponse = ( queryResult: UseClientRequestResult<AllProjectsQueryResult> ) => {
    const { loading, error, data } = queryResult

    if( loading ) {
      return
    }

    if( error ) {
      console.log( error )

      setLoadingErrors( error )
      setProjectData( noProjects )

      return
    }

    if( data ) {
      console.log( `loaded project data: ${data.allProjects.totalCount} projects` )

      setLoadingErrors( undefined )
      setProjectData( new ProjectData( data ) )
    }
  }

  useEffect( () => {
    setLoadingErrors( undefined )
    setProjectData( noProjects )

    if( user !== noUser ) {
      (async () => await requestProjects())()
    }
  }, [user, requestProjects] )

  useEffect( () => {
    processResponse( queryResult )
  }, [queryResult, queryResult.data, queryResult.error] )

  useEffect( () => {
    if( currentProject === undefined ) {
      setCurrentProject( projectData.projects.find( () => true ) )
    }
  }, [currentProject, projectData.projects] )

  return (
    <ProjectQueryContext.Provider
      value={{
        projects: projectData,
        loadingErrors: loadingErrors,
        currentProject: currentProject,
        setCurrentProject: p => setCurrentProject( p )
      }}>
      {props.children}
    </ProjectQueryContext.Provider>
  )
}

const useProjectQueryContext = () => useContext( ProjectQueryContext )

export {ProjectQueryContextProvider, useProjectQueryContext}
