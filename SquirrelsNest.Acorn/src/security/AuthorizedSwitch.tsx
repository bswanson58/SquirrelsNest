import { ReactElement, useEffect, useContext, useState } from 'react'
import AuthenticationContext from './AuthenticationContext'

interface authorizedProps {
  authorized: ReactElement
  notAuthorized?: ReactElement
  role?: string
}

export default function Authorized(props: authorizedProps) {
  const [isAuthorized, setIsAuthorized] = useState( true )
  const { user } = useContext( AuthenticationContext )

  useEffect(() => {
    if (props.role) {
      setIsAuthorized( user.hasRoleClaim(props.role))
    } else {
      setIsAuthorized(user.isLoggedIn)
    }
  }, [user, props.role])

  return <>{isAuthorized ? props.authorized : props.notAuthorized}</>
}
