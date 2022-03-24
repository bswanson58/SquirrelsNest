import { BrowserRouter as Router, Route, Routes } from 'react-router-dom'
import { ThemeProvider } from '@mui/material/styles'
import CssBaseline from '@mui/material/CssBaseline'
import DefaultPage from '../pages/DefaultPage'
import theme from '../theme'
import { AppRoute } from '../types/AppRoute'
import appRoutes from '../config/appRoutes'

function Application() {
  const addRoute = (route: AppRoute) => (
    <Route key={route.key} path={route.path} element={route.component || DefaultPage()}  />
  )

  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <Router>
        <Routes>
          {appRoutes.map((route: AppRoute) =>
            route.subRoutes
              ? route.subRoutes.map((item: AppRoute) => addRoute(item))
              : addRoute(route)
          )}
        </Routes>
      </Router>
    </ThemeProvider>
  )
}

export default Application
