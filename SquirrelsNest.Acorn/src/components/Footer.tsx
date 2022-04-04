import { AppBar } from '@mui/material'
import Typography from '@mui/material/Typography'
import { useUserContext } from '../security/UserContext'
import { Box } from '@mui/system'

function Footer() {
  const { user } = useUserContext()

  return (
    <>
      <AppBar position='fixed' sx={{ top: 'auto', bottom: 0 }}>
        <Box m={1}>
          <Typography>
            {user.name()}{' '}
            {user.isLoggedIn()
              ? user.hasRoleClaim('admin')
                ? '(admin)'
                : '(user)'
              : ''}
          </Typography>
        </Box>
      </AppBar>
    </>
  )
}

export default Footer
