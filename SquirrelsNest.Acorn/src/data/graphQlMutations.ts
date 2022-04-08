export const ADD_ISSUE_MUTATION = `mutation addIssue($issue: AddIssueInput!) {
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
