import {gql} from 'apollo-angular'

export const LoginMutation = gql`
  mutation login($loginInput: LoginInput!) {
    login( loginInput: $loginInput) {
      token
      expiration
    }
  }`
