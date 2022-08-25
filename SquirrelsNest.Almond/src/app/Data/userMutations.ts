import {gql} from 'apollo-angular'

export const AddUserMutation = gql`
  mutation AddUserMutation($userInput: AddUserInput!) {
    addUser(userInput: $userInput) {
      user {
        name
        loginName
        email
        claims {
          type
          value
        }
      }
      errors {
        message
        suggestion
      }
    }
  }
`
