import {useEffect} from 'react'
import {getAuthenticationExpiration, getAuthenticationToken} from '../security/jwtSupport'
import {authLoaded} from '../store/auth'
import {requestInitialProjects} from '../store/projectActions'
import {useAppDispatch} from '../store/storeHooks'

export function LocalStorageProvider() {
  const dispatch = useAppDispatch()

  useEffect( () => {
    const token = getAuthenticationToken()
    const expiration = getAuthenticationExpiration()

    if( (token !== null) &&
      (expiration !== null) ) {
      dispatch( authLoaded( { token, expiration: +expiration } ) )
      dispatch( requestInitialProjects() )
    }
  }, [dispatch] )

  return null
}
