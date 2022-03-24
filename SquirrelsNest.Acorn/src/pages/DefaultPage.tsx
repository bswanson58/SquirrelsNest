import { Typography, Box } from '@mui/material'
import { PageTitle } from '../components/shared/PageTitle'

function DefaultPage() {
  return (
    <>
      <PageTitle title='Navigation Error' />
      <Box sx={{ p: 3 }}>
        <Typography>This page seems to be missing in action...</Typography>
      </Box>
    </>
  )
}

export default DefaultPage
