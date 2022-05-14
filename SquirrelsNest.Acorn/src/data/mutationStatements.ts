import {gql} from 'graphql-request'

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
          name
        }
        assignedTo {
          name
        }
        workflowState {
          name
        }
        issueType {
          name
        }
      }
      errors {
        message
        suggestion
      }
    }
  }`

export const EditIssueMutation = gql`
  mutation editIssue($issue: EditIssueInput!) {
    editIssue(issue: $issue ) {
      issue {
        id
        issueNumber
        title
        description
        component {
          name
        }
        assignedTo {
          name
        }
        workflowState {
          name
        }
        issueType {
          name
        }
      }
      errors {
        message
        suggestion
      }
    }
  }
`
