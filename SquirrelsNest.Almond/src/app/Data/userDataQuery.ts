import {gql} from 'apollo-angular'

export const UserDataQuery = gql`
  query userDataQuery($dataType: UserDataType) {
    userData(dataType: $dataType) {
      userData {
        dataType
        data
        id
        userId
      }
      errors {
        message
        suggestion
      }
    }
  }
`

export const UserDataMutation = gql`
  mutation saveUserData($dataInput: UserDataInput!) {
    saveUserData(dataInput: $dataInput) {
      userData {
        dataType
        data
        id
        userId
      }
      errors {
        message
        suggestion
      }
    }
  }
`
