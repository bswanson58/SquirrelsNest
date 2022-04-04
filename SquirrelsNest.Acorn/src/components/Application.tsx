import { ThemeProvider } from '@mui/material/styles'
import CssBaseline from '@mui/material/CssBaseline'
import GraphQlContext from '../data/GraphQlContext'
import theme from '../theme'
import { ProjectContextProvider } from '../data/ProjectContext'
import { IssueContextProvider } from '../data/IssueContext'
import { UserContextProvider } from '../security/UserContext'
import ApplicationRouter from './ApplicationRouter'

function Application() {
  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <GraphQlContext>
        <UserContextProvider>
          <ProjectContextProvider>
            <IssueContextProvider>
              <ApplicationRouter />
            </IssueContextProvider>
          </ProjectContextProvider>
        </UserContextProvider>
      </GraphQlContext>
    </ThemeProvider>
  )
}

export default Application
