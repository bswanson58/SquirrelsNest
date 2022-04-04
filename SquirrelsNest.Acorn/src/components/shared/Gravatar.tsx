import { Avatar } from '@mui/material'
import { useUserContext } from '../../security/UserContext'
import gravatar from 'gravatar'

export default function () {
  const { user } = useUserContext()
  const imageUrl = gravatar.url(user.emailAddress(), { d:'identicon' })

  return <Avatar alt={user.name()} src={imageUrl} />
}
