import GraphQlContext from '../data/GraphQlContext'
import {UserContextProvider} from '../security/UserContext'
import {IssueMutationContextProvider} from './IssueMutationContext'
import {ProjectQueryContextProvider} from './ProjectQueryContext'
import {IssueQueryContextProvider} from './IssueQueryContext'
import {Provider} from 'react-redux'
import {appStore} from '../store/configureStore'

function ContextProviders( props: any ) {
  return (
    <Provider store={appStore}>
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
    </Provider>
  )
}

export {ContextProviders}
