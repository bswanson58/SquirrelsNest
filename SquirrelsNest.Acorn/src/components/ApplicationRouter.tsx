import {BrowserRouter as Router, Route, Routes} from 'react-router-dom'
import {hasRoleClaim} from '../security/userClaims'
import {selectUserClaims} from '../store/auth'
import {useAppSelector} from '../store/storeHooks'
import {AppRoute} from '../types/AppRoute'
import {appRoutes} from '../config/appRoutes'
import DefaultPage from './DefaultPage'
import UnauthorizedPage from './UnauthorizedPage'

function ApplicationRouter() {
  const userClaims = useAppSelector(selectUserClaims)

  const addRoute = ( route: AppRoute ): JSX.Element => {
    return (
      <Route
        key={route.key}
        path={route.path}
        element={
          hasRoleClaim(userClaims, route.roleClaim ) ? (
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
