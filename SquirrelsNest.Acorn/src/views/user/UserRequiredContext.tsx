import {useEffect} from 'react'
import {useNavigate} from 'react-router-dom'
import {loginUrl} from '../../config/appRoutes'
import {selectIsUserAuthenticated} from '../../store/auth'
import {useAppSelector} from '../../store/storeHooks'

function useUserRequired( redirectUrl = loginUrl ) {
  const isAuthenticated = useAppSelector( selectIsUserAuthenticated )
  const navigate = useNavigate()

  useEffect( () => {
    if( isAuthenticated ) {
      navigate( redirectUrl )
    }
  }, [isAuthenticated, navigate, redirectUrl] )

  return isAuthenticated
}

export {useUserRequired}
