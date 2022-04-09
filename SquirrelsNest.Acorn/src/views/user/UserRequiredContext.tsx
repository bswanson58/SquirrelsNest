import {useEffect} from 'react'
import {useNavigate} from 'react-router-dom'
import {loginUrl} from '../../config/appRoutes'
import {useUserContext} from '../../security/UserContext'

function useUserRequired( redirectUrl = loginUrl ) {
  const userContext = useUserContext()
  const navigate = useNavigate()

  useEffect( () => {
    if(!userContext.user.isLoggedIn()) {
      navigate( redirectUrl )
    }
  }, [userContext, navigate, redirectUrl] )

  return userContext
}

export {useUserRequired}
