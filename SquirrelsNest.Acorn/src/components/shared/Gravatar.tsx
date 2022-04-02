import { useContext } from 'react'
import { Avatar } from '@mui/material'
import UserContext from '../../security/UserContext'
import gravatar from 'gravatar'

export default function () {
  const { user } = useContext(UserContext)
  const imageUrl = gravatar.url(user.emailAddress(), { d:'identicon' })

  return <Avatar alt={user.name()} src={imageUrl} />
}
