import { Avatar } from '@mui/material'
import { useUserContext } from '../../security/UserContext'
import gravatar from 'gravatar'

function Gravatar() {
  const { user } = useUserContext()
  const imageUrl = gravatar.url(user.emailAddress(), { d:'identicon' })

  return <Avatar alt={user.name()} src={imageUrl} />
}

export default Gravatar
