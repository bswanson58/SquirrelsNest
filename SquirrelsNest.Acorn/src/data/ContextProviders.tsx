import GraphQlContext from '../data/GraphQlContext'
import { UserContextProvider } from '../security/UserContext'
import {IssueMutationContextProvider} from './IssueMutationContext'
import { ProjectQueryContextProvider } from './ProjectQueryContext'
import { IssueQueryContextProvider } from './IssueQueryContext'

function ContextProviders(props: any) {
  return (
    <GraphQlContext>
      <UserContextProvider>
        <ProjectQueryContextProvider>
          <IssueQueryContextProvider>
            <IssueMutationContextProvider>
              {props.children}
            </IssueMutationContextProvider>
          </IssueQueryContextProvider>
        </ProjectQueryContextProvider>
      </UserContextProvider>
    </GraphQlContext>
  )
}

export { ContextProviders }
