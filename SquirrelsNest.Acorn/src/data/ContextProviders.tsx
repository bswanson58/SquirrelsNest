import GraphQlContext from '../data/GraphQlContext'
import { UserContextProvider } from '../security/UserContext'
import { ProjectContextProvider } from './ProjectContext'
import { IssueQueryContextProvider } from './IssueQueryContext'

function ContextProviders(props: any) {
  return (
    <GraphQlContext>
      <UserContextProvider>
        <ProjectContextProvider>
          <IssueQueryContextProvider>
              {props.children}
          </IssueQueryContextProvider>
        </ProjectContextProvider>
      </UserContextProvider>
    </GraphQlContext>
  )
}

export { ContextProviders }
