import {Box, Typography} from '@mui/material'
import axios from 'axios'
import styled from 'styled-components'
import {parseAxiosError} from '../utility/axiosErrorParser'
import {useState} from 'react'
import {useNavigate} from 'react-router-dom'
import {urlAccounts} from '../config/endpoints'
import {authenticationResponse, userCredentials} from '../security/authenticationModels'
import AuthenticationForm from './AuthenticationForm'
import ErrorDisplay from './ErrorDisplay'

const BorderedBox = styled( Box )`
  margin: 50px;
`

function RegisterUser() {
  const [errors, setErrors] = useState<string[]>( [] )
  const history = useNavigate()

  async function register( credentials: userCredentials ) {
    try {
      setErrors( [] )
      await axios.post<authenticationResponse>( `${urlAccounts}/create`, credentials )
      history( '/' )
    } catch( error ) {
      setErrors( parseAxiosError( error ) )
    }
  }

  return (
    <BorderedBox>
      <Typography variant="h6">Register</Typography>
      <ErrorDisplay errors={errors}/>
      <AuthenticationForm
        requireName={true}
        submitText={'Register'}
        model={{ name: '', email: '', password: '' }}
        onSubmit={async values => await register( values )}
      />
    </BorderedBox>
  )
}

export default RegisterUser
