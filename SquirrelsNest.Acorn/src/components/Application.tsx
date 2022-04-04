import { ThemeProvider } from '@mui/material/styles'
import CssBaseline from '@mui/material/CssBaseline'
import theme from '../theme'
import { ContextProviders } from '../data/ContextProviders'
import ApplicationRouter from './ApplicationRouter'

function Application() {
  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <ContextProviders>
        <ApplicationRouter />
      </ContextProviders>
    </ThemeProvider>
  )
}

export default Application
