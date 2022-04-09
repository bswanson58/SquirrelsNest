import {BrowserRouter as Router, Route, Routes} from 'react-router-dom'
import {AppRoute} from '../types/AppRoute'
import {appRoutes} from '../config/appRoutes'
import DefaultPage from './DefaultPage'
import {useUserContext} from '../security/UserContext'
import UnauthorizedPage from './UnauthorizedPage'

function ApplicationRouter() {
  const { user } = useUserContext()

  const addRoute = ( route: AppRoute ): JSX.Element => {
    return (
      <Route
        key={route.key}
        path={route.path}
        element={
          user.hasRoleClaim( route.roleClaim ) ? (
            route.component || DefaultPage()
          ) : (
            <UnauthorizedPage/>
          )
        }
      />
    )
  }

  return (
    <Router>
      <Routes>
        {appRoutes.map( ( route: AppRoute ) =>
          route.subRoutes
            ? route.subRoutes.map( ( item: AppRoute ) => addRoute( item ) )
            : addRoute( route )
        )}
      </Routes>
    </Router>
  )
}

export default ApplicationRouter
