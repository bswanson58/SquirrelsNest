import axios from 'axios'
import styled from 'styled-components'
import {parseAxiosError} from '../../utility/axiosErrorParser'
import {authenticationResponse, userCredentials} from '../../security/authenticationModels'
import {urlAccounts} from '../../config/endpoints'
import {useState} from 'react'
import {useNavigate} from 'react-router-dom'
import {getAuthenticationClaims, saveAuthenticationToken} from '../../security/jwtSupport'
import AuthenticationForm from './AuthenticationForm'
import {useUserContext} from '../../security/UserContext'
import ErrorDisplay from '../../components/ErrorDisplay'
import {User} from '../../security/user'
import {Box, Typography} from '@mui/material'

const BorderedBox = styled(Box) `
  margin: 50px;
`

export default function Login() {
  const [errors, setErrors] = useState<string[]>( [] )
  const { updateUser } = useUserContext()
  const history = useNavigate()

  async function login( credentials: userCredentials ) {
    try {
      setErrors( [] )
      const response = await axios.post<authenticationResponse>(
        `${urlAccounts}/login`,
        credentials
      )
      saveAuthenticationToken( response.data )
      updateUser( new User( getAuthenticationClaims() ) )
      history( '/' )
    } catch( error ) {
      setErrors( parseAxiosError( error ) )
    }
  }

  return (
    <BorderedBox>
      <Typography variant="h6">Login</Typography>
      <ErrorDisplay errors={errors}/>
      <AuthenticationForm
        requireName={false}
        submitText={'Login'}
        model={{ name: '', email: '', password: '' }}
        onSubmit={async ( values ) => await login( values )}
      />
    </BorderedBox>
  )
}
