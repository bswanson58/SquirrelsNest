import axios, { AxiosError } from 'axios'
import { authenticationResponse,  userCredentials } from '../security/authenticationModels'
import AuthenticationForm from './AuthenticationForm'
import { urlAccounts } from '../config/endpoints'
import { useContext, useState } from 'react'
import ErrorDisplay from './ErrorDisplay'
import { getAuthenticationClaims, saveAuthenticationToken } from '../security/jwtSupport'
import AuthenticationContext from '../security/AuthenticationContext'
import { useNavigate } from 'react-router-dom'

export default function Login() {
  const [errors, setErrors] = useState<string[]>([])
  const { update } = useContext(AuthenticationContext)
  const history = useNavigate()

  async function login(credentials: userCredentials) {
    try {
      setErrors([])
      const response = await axios.post<authenticationResponse>(
        `${urlAccounts}/login`,
        credentials
      )
      saveAuthenticationToken(response.data)
      update(getAuthenticationClaims())
      history('/')
    } catch (error) {
      if (axios.isAxiosError(error)) {
        const err = error as AxiosError

        setErrors(err.response?.data)
      }
    }
  }

  return (
    <>
      <h3>Login</h3>
      <ErrorDisplay errors={errors} />
      <AuthenticationForm
        model={{ email: '', password: '' }}
        onSubmit={async (values) => await login(values)}
      />
    </>
  )
}
