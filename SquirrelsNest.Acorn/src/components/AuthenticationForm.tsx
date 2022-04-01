import {userCredentials} from '../security/authenticationModels'
import {Form, Formik, FormikHelpers} from 'formik';
import * as Yup from 'yup';
import TextField from './shared/TextField';
import Button from './shared/Button';
import { Link } from 'react-router-dom';

interface authFormProps{
    model: userCredentials
    requireName: boolean
    onSubmit(values: userCredentials, actions: FormikHelpers<userCredentials>): void
}

export default function AuthForm(props: authFormProps){
    return (
        <Formik
            initialValues={props.model}
            onSubmit={props.onSubmit}
            validationSchema={Yup.object({
                name: props.requireName ? 
                    Yup.string().required('User name is required') :
                    Yup.string().notRequired(),
                email: Yup.string().required('An email address is required')
                    .email('Entry must be a valid email'),
                password: Yup.string().required('A password is required')
            })}
        >
            {formikProps => (
                <Form>
                    { props.requireName && <TextField displayName="Name" field='name' />}
                    <TextField displayName="Email" field="email" />
                    <TextField displayName="Password" field="password" type="password" />

                    <Button disabled={formikProps.isSubmitting} type="submit">Send</Button>
                    <Link className="btn btn-secondary" to="/">Cancel</Link>
                </Form>
            )}
        </Formik>
    )
}
