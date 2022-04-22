import {ClIssue} from './graphQlTypes'

export interface AddIssueInput {
  projectId: String
  title: String
  description: String
}

export interface AddIssuePayload {
  addIssue: {
    issue: ClIssue
    errors: MutationError[]
  }
}

export interface MutationError {
  message: String
  suggestion: String
}
