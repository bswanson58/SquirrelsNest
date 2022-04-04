import { useContext } from 'react'
import Avatar from '@mui/material/Avatar'
import Box from '@mui/material/Box'
import Grid from '@mui/material/Grid'
import { Stack } from '@mui/material'
import UserContext from '../security/UserContext'

function UserInfo() {
  const { user } = useContext(UserContext)

  return (
    <Grid
      container
      justifyContent='center'
      sx={{ padding: '15px', bgcolor: 'red' }}>
      <Stack>
        <Avatar sx={{ width: 100, height: 100 }}>BS</Avatar>
        <Box style={{ display: 'flex', justifyContent: 'center' }}>
          <p>{user.name}</p>
        </Box>
      </Stack>
    </Grid>
  )
}

export default UserInfo
