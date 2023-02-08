import { AppBar } from '@mui/material'
import Typography from '@mui/material/Typography'
import { Box } from '@mui/system'
import {selectUserName} from '../store/auth'
import {useAppSelector} from '../store/storeHooks'

function Footer() {
  const userName = useAppSelector( selectUserName )

  return (
    <>
      <AppBar position='fixed' sx={{ top: 'auto', bottom: 0 }}>
        <Box m={1}>
          <Typography>
            {userName}
          </Typography>
        </Box>
      </AppBar>
    </>
  )
}

export default Footer
