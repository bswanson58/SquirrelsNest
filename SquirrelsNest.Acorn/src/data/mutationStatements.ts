import {gql} from 'graphql-request'

export const LoginMutation = gql`
  mutation login($loginInput: LoginInput!) {
    login( loginInput: $loginInput) {
      token
      expiration
    }
  }`

export const AddIssueMutation = gql`
  mutation addIssue($issue: AddIssueInput!) {
    addIssue( issue: $issue ) {
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
