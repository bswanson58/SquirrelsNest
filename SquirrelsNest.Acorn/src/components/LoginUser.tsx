import axios from 'axios'
import { parseAxiosError } from '../utility/axiosErrorParser'
import { authenticationResponse, userCredentials } from '../security/authenticationModels'
import { urlAccounts } from '../config/endpoints'
import { useContext, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { getAuthenticationClaims, saveAuthenticationToken } from '../security/jwtSupport'
import AuthenticationForm from './AuthenticationForm'
import UserContext from '../security/UserContext'
import ErrorDisplay from './ErrorDisplay'
import { User } from '../security/user'

export default function Login() {
  const [errors, setErrors] = useState<string[]>([])
  const { updateUser } = useContext(UserContext)
  const history = useNavigate()

  async function login(credentials: userCredentials) {
    try {
      setErrors([])
      const response = await axios.post<authenticationResponse>(
        `${urlAccounts}/login`,
        credentials
      )
      saveAuthenticationToken(response.data)
      updateUser( new User(getAuthenticationClaims()))
      history('/')
    } catch (error) {
      setErrors(parseAxiosError(error))
    }
  }

  return (
    <>
      <h3>Login</h3>
      <ErrorDisplay errors={errors} />
      <AuthenticationForm
        requireName={false}
        model={{ name: '', email: '', password: '' }}
        onSubmit={async (values) => await login(values)}
      />
    </>
  )
}
