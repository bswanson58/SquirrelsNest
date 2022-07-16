import {gql} from 'apollo-angular'

export const LoginMutation = gql`
  mutation login($loginInput: LoginInput!) {
    login( loginInput: $loginInput) {
      token
      expiration
    }
  }`

export const AddIssueMutation = gql`
  mutation addIssue($issueInput: AddIssueInput!) {
    addIssue( issueInput: $issueInput ) {
      issue {
        id
        issueNumber
        title
        description
        component {
          id
          name
        }
        assignedTo {
          id
          name
        }
        workflowState {
          id
          name
          category
        }
        issueType {
          id
          name
        }
      }
      errors {
        message
        suggestion
      }
    }
  }`

export const UpdateIssueMutation = gql`
  mutation updateIssue($updateInput: UpdateIssueInput!) {
    updateIssue(updateInput: $updateInput ) {
      issue {
        id
        issueNumber
        title
        description
        component {
          id
          name
        }
        assignedTo {
          id
          name
        }
        workflowState {
          id
          name
          category
        }
        issueType {
          id
          name
        }
      }
      errors {
        message
        suggestion
      }
    }
  }`

export const DeleteIssueMutation = gql`
  mutation deleteIssue($deleteInput: DeleteIssueInput!) {
    deleteIssue(deleteInput: $deleteInput) {
      issueId
      errors {
        message
        suggestion
      }
    }
  }`
