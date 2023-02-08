import { Avatar } from '@mui/material'
import gravatar from 'gravatar'
import {selectUserEmail, selectUserName} from '../../store/auth'
import {useAppSelector} from '../../store/storeHooks'

function Gravatar() {
  const userEmail = useAppSelector( selectUserEmail )
  const userName = useAppSelector( selectUserName )
  const imageUrl = gravatar.url(userEmail, { d:'identicon' })

  return <Avatar alt={userName} src={imageUrl} />
}

export default Gravatar
