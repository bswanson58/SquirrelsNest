import axios from 'axios'
import { parseAxiosError } from '../utility/axiosErrorParser';
import { useState, useContext } from "react";
import { useNavigate } from "react-router-dom";
import { urlAccounts } from '../config/endpoints';
import { authenticationResponse, userCredentials } from "../security/authenticationModels";
import AuthenticationContext from "../security/AuthenticationContext";
import AuthenticationForm from './AuthenticationForm'
import ErrorDisplay from './ErrorDisplay';

function RegisterUser() {
    const [errors, setErrors] = useState<string[]>([]);
    const {update} = useContext(AuthenticationContext);
    const history = useNavigate();

    async function register(credentials: userCredentials) {
        try{
            setErrors([]);
            const response = await axios
                .post<authenticationResponse>(`${urlAccounts}/create`, credentials)
            history('/')
        }
        catch(error) {
            setErrors(parseAxiosError(error))
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