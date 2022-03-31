import axios, { AxiosError } from 'axios'
import { useState, useContext } from "react";
import { useNavigate } from "react-router-dom";
import { urlAccounts } from '../config/endpoints';
import { authenticationResponse, userCredentials } from "../security/authenticationModels";
import { getAuthenticationClaims, saveAuthenticationToken } from '../security/jwtSupport';
import AuthenticationContext from "../security/AuthenticationContext";
import AuthenticationForm from './AuthenticationForm'
import ErrorDisplay from './ErrorDisplay';

function RegisterUser() {
    const [errors, setErrors] = useState<string[]>([]);
    const {update} = useContext(AuthenticationContext);
    const history = useNavigate();

    async function register(credentials: userCredentials) {
        try{
            console.log( credentials )

            setErrors([]);
            const response = await axios
                .post<authenticationResponse>(`${urlAccounts}/create`, credentials)
            saveAuthenticationToken(response.data)
            update(getAuthenticationClaims())
            history('/')
        }
        catch(error) {
            if (axios.isAxiosError(error)) {
                const err = error as AxiosError

                setErrors(err.response?.data)
            }
        }
    }

    return (
        <>
            <h3>Register User</h3>
            <ErrorDisplay errors={errors} />
            <AuthenticationForm
            model={{email: '', password: ''}}
            onSubmit={async values => await register(values)}
            />
        </>
    )
}

export default RegisterUser