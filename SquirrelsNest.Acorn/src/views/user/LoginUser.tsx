import {useEffect} from 'react'
import styled from 'styled-components'
import {userCredentials} from '../../security/authenticationModels'
import {useNavigate} from 'react-router-dom'
import {selectIsAuthenticating, selectIsUserAuthenticated} from '../../store/auth'
import {loginUser} from '../../store/authActions'
import {useAppDispatch, useAppSelector} from '../../store/storeHooks'
import AuthenticationForm from './AuthenticationForm'
import {Box, Typography} from '@mui/material'

const BorderedBox = styled( Box )`
  margin: 50px;
`
export default function Login() {
  const navigate = useNavigate()
  const dispatch = useAppDispatch()
  const isLoggedIn = useAppSelector(selectIsUserAuthenticated)
  const isAuthenticating = useAppSelector(selectIsAuthenticating)

  useEffect(() => {
    if(isLoggedIn) {
      navigate('/')
    }
  }, [isLoggedIn])

  function handleLogin(credentials: userCredentials) {
    dispatch(loginUser(credentials))
  }

  return (
    <BorderedBox>
      <Typography variant='h6'>Login</Typography>
      <AuthenticationForm
        requireName={false}
        submitText={'Login'}
        model={{ name: '', email: '', password: '' }}
        onSubmit={( values ) => handleLogin( values )}
      />

      {isAuthenticating && <Typography>Is Authenticating...</Typography>}
    </BorderedBox>
  )
}
