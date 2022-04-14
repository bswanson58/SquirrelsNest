import styled from 'styled-components'
import {LoginResponse} from '../../data/GraphQlEntities'
import {LOGIN_QUERY} from '../../data/GraphQlQueries'
import {userCredentials} from '../../security/authenticationModels'
import {useContext, useState} from 'react'
import {useNavigate} from 'react-router-dom'
import {getAuthenticationClaims, saveAuthenticationToken} from '../../security/jwtSupport'
import AuthenticationForm from './AuthenticationForm'
import {useUserContext} from '../../security/UserContext'
import {User} from '../../security/user'
import {Box, Typography} from '@mui/material'
import {useManualQuery, ClientContext, APIError} from 'graphql-hooks'

const BorderedBox = styled( Box )`
  margin: 50px;
`

export default function Login() {
  const [loadingErrors, setLoadingErrors] = useState<APIError>()
  const { updateUser } = useUserContext()
  const history = useNavigate()
  const clientContext = useContext( ClientContext )

  const [loginRequest] = useManualQuery<LoginResponse>( LOGIN_QUERY )

  async function handleLogin( credentials: userCredentials ) {
    const { data, error } = await loginRequest( {
      variables: { credentials: { email: credentials.email, password: credentials.password, name: '' } }
    } )
    if( error ) {
      setLoadingErrors( error )

      return
    }

    if( data ) {
      saveAuthenticationToken( data.login )
      updateUser( new User( getAuthenticationClaims() ) )
      clientContext.setHeader( 'Authorization', `Bearer ${data.login.token}` )

      history( '/' )
    }
  }

  return (
    <BorderedBox>
      <Typography variant='h6'>Login</Typography>
      <AuthenticationForm
        requireName={false}
        submitText={'Login'}
        model={{ name: '', email: '', password: '' }}
        onSubmit={async ( values ) => await handleLogin( values )}
      />
    </BorderedBox>
  )
}
