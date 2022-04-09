import {Box} from '@mui/system'
import {useUserRequired} from '../views/user/UserRequiredContext'

export default function UnauthorizedPage() {
//  useUserRequired()

  return(
    <Box>You are not authorized to view this page</Box>
  )
}
