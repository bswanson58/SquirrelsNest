import {gql} from 'apollo-angular'

export const AddProjectMutation = gql`
  mutation addProject($addInput: AddProjectInput!) {
    addProject(projectInput: $addInput) {
      project {
        id
        name
        description
        issuePrefix
        components {
          id
          name
          description
        }
        issueTypes {
          id
          name
          description
        }
        workflowStates {
          id
          name
          description
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

export const UpdateProjectMutation = gql`
  mutation updateProject($updateInput: UpdateProjectInput!) {
    updateProject(updateInput: $updateInput){
      project {
        id
        name
        description
        issuePrefix
        components {
          id
          name
          description
        }
        issueTypes {
          id
          name
          description
        }
        workflowStates {
          id
          name
          description
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

export const DeleteProjectMutation = gql`
  mutation deleteProject($deleteInput: DeleteProjectInput!) {
    deleteProject(deleteInput: $deleteInput) {
      projectId
      errors {
        message
        suggestion
      }
    }
  }`

export const CreateProjectTemplate = gql`
  mutation createProjectTemplate($templateInput: CreateTemplateInput!) {
    createProjectTemplate(templateInput: $templateInput) {
      succeeded
      errors {
        message
        suggestion
      }
    }
  }`
