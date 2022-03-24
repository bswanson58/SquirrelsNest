import Avatar from '@mui/material/Avatar'
import Box from '@mui/material/Box'
import Grid from '@mui/material/Grid'
import { Stack } from '@mui/material'

function UserInfo() {
  return (
    <Grid
      container
      justifyContent='center'
      sx={{ padding: '15px', bgcolor: 'red' }}>
      <Stack>
        <Avatar sx={{ width: 100, height: 100 }}>BS</Avatar>
        <Box style={{ display: 'flex', justifyContent: 'center' }}>
          <p>Bill Swanson</p>
        </Box>
      </Stack>
    </Grid>
  )
}

export default UserInfo
