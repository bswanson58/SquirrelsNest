import {Provider} from 'react-redux'
import {appStore} from '../store/configureStore'

function ContextProviders( props: any ) {
  return (
    <Provider store={appStore}>
      {props.children}
    </Provider>
  )
}

export {ContextProviders}
