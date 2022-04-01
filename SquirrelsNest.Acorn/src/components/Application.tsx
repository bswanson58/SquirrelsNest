import { BrowserRouter as Router, Route, Routes } from 'react-router-dom'
import { ThemeProvider } from '@mui/material/styles'
import CssBaseline from '@mui/material/CssBaseline'
import DefaultPage from '../pages/DefaultPage'
import GraphQlContext from '../data/GraphQlContext'
import { claim } from '../security/authenticationModels'
import theme from '../theme'
import { AppRoute } from '../types/AppRoute'
import appRoutes from '../config/appRoutes'
import { useState, useEffect } from 'react'
import AuthenticationContext from '../security/AuthenticationContext'
import { getAuthenticationClaims, hasRoleClaim } from '../security/jwtSupport'

function Application() {
  const [claims, setClaims] = useState<claim[]>([
    {name: 'email', value:'bswanson58@gmail.com'},
    {name: 'role', value: 'user'}
  ])

//  useEffect(() => {
//    setClaims( getAuthenticationClaims())
//  }, [])

  const addRoute = (route: AppRoute): JSX.Element => {
    return (
      <Route
        key={route.key}
        path={route.path}
        element={
          hasRoleClaim(route.roleClaim, claims) ? (
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
        <AuthenticationContext.Provider value={{ claims, update: setClaims }}>
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
