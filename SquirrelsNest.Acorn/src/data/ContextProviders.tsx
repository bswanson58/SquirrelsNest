import GraphQlContext from '../data/GraphQlContext'
import { UserContextProvider } from '../security/UserContext'
import { ProjectContextProvider } from './ProjectContext'
import { IssueContextProvider } from './IssueContext'

function ContextProviders(props: any) {
  return (
    <GraphQlContext>
      <UserContextProvider>
        <ProjectContextProvider>
          <IssueContextProvider>
              {props.children}
          </IssueContextProvider>
        </ProjectContextProvider>
      </UserContextProvider>
    </GraphQlContext>
  )
}

export { ContextProviders }
