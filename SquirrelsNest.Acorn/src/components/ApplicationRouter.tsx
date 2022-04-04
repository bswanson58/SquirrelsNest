import { BrowserRouter as Router, Route, Routes } from 'react-router-dom'
import { AppRoute } from '../types/AppRoute'
import appRoutes from '../config/appRoutes'
import DefaultPage from '../pages/DefaultPage'
import { useContext } from 'react'
import UserContext from '../security/UserContext'

function ApplicationRouter() {
  const { user } = useContext(UserContext)

  const addRoute = (route: AppRoute): JSX.Element => {
    return (
      <Route
        key={route.key}
        path={route.path}
        element={
          user.hasRoleClaim(route.roleClaim) ? (
            route.component || DefaultPage()
          ) : (
            <p>You are not authorized to view this page</p>
          )
        }
      />
    )
  }

  return (
    <Router>
      <Routes>
        {appRoutes.map((route: AppRoute) =>
          route.subRoutes
            ? route.subRoutes.map((item: AppRoute) => addRoute(item))
            : addRoute(route)
        )}
      </Routes>
    </Router>
  )
}

export default ApplicationRouter
