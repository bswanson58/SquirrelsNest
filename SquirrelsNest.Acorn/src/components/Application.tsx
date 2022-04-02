import { BrowserRouter as Router, Route, Routes } from 'react-router-dom'
import { ThemeProvider } from '@mui/material/styles'
import CssBaseline from '@mui/material/CssBaseline'
import DefaultPage from '../pages/DefaultPage'
import GraphQlContext from '../data/GraphQlContext'
import theme from '../theme'
import { AppRoute } from '../types/AppRoute'
import appRoutes from '../config/appRoutes'
import { useState, useEffect } from 'react'
import AuthenticationContext from '../security/AuthenticationContext'
import { User, noUser } from '../security/user'

function Application() {
  const [user, setUser] = useState<User>(noUser)

//  useEffect(() => {
//    setClaims( getAuthenticationClaims())
//  }, [])

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
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <GraphQlContext>
        <AuthenticationContext.Provider value={{ user, updateUser: setUser }}>
          <Router>
            <Routes>
              {appRoutes.map((route: AppRoute) =>
                route.subRoutes
                  ? route.subRoutes.map((item: AppRoute) => addRoute(item))
                  : addRoute(route)
              )}
            </Routes>
          </Router>
        </AuthenticationContext.Provider>
      </GraphQlContext>
    </ThemeProvider>
  )
}

export default Application
