import { ReactElement, useEffect, useState } from 'react'
import { useUserContext } from './UserContext'

interface authorizedProps {
  authorized: ReactElement
  notAuthorized?: ReactElement
  role?: string
}

function AuthorizedSwitch(props: authorizedProps) {
  const [isAuthorized, setIsAuthorized] = useState( true )
  const { user } = useUserContext()

  useEffect(() => {
    if (props.role) {
      setIsAuthorized( user.hasRoleClaim( props.role ))
    } else {
      setIsAuthorized( user.isLoggedIn )
    }
  }, [user, props.role])

  return <>{isAuthorized ? props.authorized : props.notAuthorized}</>
}

export default AuthorizedSwitch