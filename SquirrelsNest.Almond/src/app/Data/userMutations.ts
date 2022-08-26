import {gql} from 'apollo-angular'

export const AddUserMutation = gql`
  mutation AddUserMutation($userInput: AddUserInput!) {
    addUser(userInput: $userInput) {
      user {
        id
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

export const DeleteUserMutation = gql`
  mutation DeleteUserMutation($deleteInput: DeleteUserInput!) {
    deleteUser(deleteInput: $deleteInput) {
      email
      errors {
        message
        suggestion
      }
    }
  }
`

export const EditUserRolesMutation = gql`
  mutation EditUserRoles($rolesInput: EditUserRolesInput!) {
    editUserRoles(rolesInput: $rolesInput) {
      user {
        id
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
