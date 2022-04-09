import {userCredentials} from '../../security/authenticationModels'
import {Field, Form, Formik, FormikHelpers} from 'formik'
import * as Yup from 'yup'
import {TextField} from 'formik-mui'
import {Button} from '@mui/material'
import styled from 'styled-components'

interface authFormProps {
  model: userCredentials
  submitText: string
  requireName: boolean

  onSubmit( values: userCredentials, actions: FormikHelpers<userCredentials> ): void
}

const WideField = styled(Field) `
  width: 400px;
  margin: 0 0 20px 10px;
`

const RightButton = styled(Button) `
  margin-top: 10px;
  width: 120px;
  margin-left: 290px;
`

function AuthenticationForm( props: authFormProps ) {
  return (
    <Formik
      initialValues={props.model}
      onSubmit={props.onSubmit}
      validationSchema={Yup.object( {
        name: props.requireName ?
          Yup.string().required( 'User name is required' ) :
          Yup.string().notRequired(),
        email: Yup.string().required( 'An email address is required' )
          .email( 'Entry must be a valid email' ),
        password: Yup.string().required( 'A password is required' )
      } )}
    >

      {( { submitForm, isSubmitting } ) => (
        <Form>
          {props.requireName &&
              <WideField component={TextField} name="Name" type="text" label="Name"/>}
          <WideField
            component={TextField}
            name="email"
            type="email"
            label="Email"
          />
          <WideField
            component={TextField}
            name="password"
            type="password"
            label="Password"
          />
          <RightButton
            variant="contained"
            color="primary"
            disabled={isSubmitting}
            onClick={submitForm}
          >
            {props.submitText}
          </RightButton>
        </Form>
      )}
    </Formik>
  )
}

export default AuthenticationForm
