import { useContext } from 'react'
import UserContext from "../security/UserContext"

function Footer() {
  const { user } = useContext(UserContext)

  return (
    <>
    <div>Footer - {user.name()} {user.isLoggedIn() ? user.hasRoleClaim('admin') ? '(admin)' : '(user)' : ''}</div>
    </>
  )
}

export default Footer