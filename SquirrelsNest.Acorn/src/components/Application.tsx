import { ThemeProvider } from '@mui/material/styles'
import CssBaseline from '@mui/material/CssBaseline'
import GraphQlContext from '../data/GraphQlContext'
import theme from '../theme'
import { useState, useEffect } from 'react'
import UserContext from '../security/UserContext'
import { User, noUser, adminUser, normalUser } from '../security/user'
import { ProjectContextProvider } from '../data/ProjectContext'
import { IssueContextProvider } from '../data/IssueContext'
import { getAuthenticationClaims } from '../security/jwtSupport'
import ApplicationRouter from './ApplicationRouter'

function Application() {
  const [user, setUser] = useState<User>(noUser)

  useEffect(() => {
    setUser(new User(getAuthenticationClaims()))
  }, [])

  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <GraphQlContext>
        <UserContext.Provider value={{ user, updateUser: setUser }}>
          <ProjectContextProvider>
            <IssueContextProvider>
              <ApplicationRouter />
            </IssueContextProvider>
          </ProjectContextProvider>
        </UserContext.Provider>
      </GraphQlContext>
    </ThemeProvider>
  )
}

export default Application
