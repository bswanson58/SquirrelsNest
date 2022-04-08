import GraphQlContext from '../data/GraphQlContext'
import { UserContextProvider } from '../security/UserContext'
import { ProjectQueryContextProvider } from './ProjectQueryContext'
import { IssueQueryContextProvider } from './IssueQueryContext'

function ContextProviders(props: any) {
  return (
    <GraphQlContext>
      <UserContextProvider>
        <ProjectQueryContextProvider>
          <IssueQueryContextProvider>
              {props.children}
          </IssueQueryContextProvider>
        </ProjectQueryContextProvider>
      </UserContextProvider>
    </GraphQlContext>
  )
}

export { ContextProviders }
