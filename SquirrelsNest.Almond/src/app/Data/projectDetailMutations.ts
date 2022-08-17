import {gql} from 'apollo-angular'

export const AddProjectDetailMutation = gql`
  mutation addProjectDetail($detailInput: ProjectDetailInput!) {
    addProjectDetail(detailInput: $detailInput) {
      project {
        id
        name
        description
        issuePrefix
        components {
          id
          name
        }
        issueTypes {
          id
          name
        }
        workflowStates {
          id
          name
          category
        }
        users {
          id
          email
          name
        }
      }
      errors {
        message
        suggestion
      }
    }
  }`

export const DeleteProjectDetailMutation = gql`
  mutation deleteProjectDetail($detailInput: ProjectDetailInput!){
    deleteProjectDetail(detailInput: $detailInput) {
      project {
        id
        name
        description
        issuePrefix
        components {
          id
          name
        }
        issueTypes {
          id
          name
        }
        workflowStates {
          id
          name
          category
        }
        users {
          id
          email
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
