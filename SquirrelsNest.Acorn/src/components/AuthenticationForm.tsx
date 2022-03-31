import {userCredentials} from '../security/authenticationModels'
import {Form, Formik, FormikHelpers} from 'formik';
import * as Yup from 'yup';
import TextField from './shared/TextField';
import Button from './shared/Button';
import { Link } from 'react-router-dom';

interface authFormProps{
    model: userCredentials;
    onSubmit(values: userCredentials, actions: FormikHelpers<userCredentials>): void; 
}
export default function AuthForm(props: authFormProps){
    return (
        <Formik
            initialValues={props.model}
            onSubmit={props.onSubmit}
            validationSchema={Yup.object({
                email: Yup.string().required('This field is required')
                    .email('You have to insert a valid email'),
                password: Yup.string().required('This field is required')
            })}
        >
            {formikProps => (
                <Form>
                    <TextField displayName="Email" field="email" />
                    <TextField displayName="Password" field="password" type="password" />

                    <Button disabled={formikProps.isSubmitting} type="submit">Send</Button>
                    <Link className="btn btn-secondary" to="/">Cancel</Link>
                </Form>
            )}
        </Formik>
    )
}
