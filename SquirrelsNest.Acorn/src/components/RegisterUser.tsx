import axios from 'axios'
import { parseAxiosError } from '../utility/axiosErrorParser';
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { urlAccounts } from '../config/endpoints';
import { authenticationResponse, userCredentials } from "../security/authenticationModels";
import AuthenticationForm from './AuthenticationForm'
import ErrorDisplay from './ErrorDisplay';

function RegisterUser() {
    const [errors, setErrors] = useState<string[]>([]);
    const history = useNavigate();

    async function register(credentials: userCredentials) {
        try{
            setErrors([]);
            await axios.post<authenticationResponse>(`${urlAccounts}/create`, credentials);
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
            requireName={true}
            model={{name: '', email: '', password: ''}}
            onSubmit={async values => await register(values)}
            />
        </>
    )
}

export default RegisterUser