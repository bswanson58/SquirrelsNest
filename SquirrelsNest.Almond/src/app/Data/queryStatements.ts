import {gql} from 'apollo-angular'
import {ClIssueSortInput, ClProjectSortInput, ClUserSortInput, Query} from './graphQlTypes'

export interface ProjectQueryInput {
  skip: number,
  take: number,
  order: ClProjectSortInput
}

export const AllProjectsQuery = gql<Query, ProjectQueryInput>`
  query projectsQuery($skip: Int!, $take: Int!, $order:[ClProjectSortInput!], $where:ClProjectFilterInput) {
    projectList(
      skip: $skip,
      take: $take,
      order: $order,
      where: $where ) {
      pageInfo {
        hasNextPage
        hasPreviousPage
      }
      items {
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
      totalCount
    }
  }`

export interface IssueQueryInput {
  projectId: string,
  skip: number,
  take: number,
  order: ClIssueSortInput
}

export const IssuesQuery = gql`
  query issuesQuery($skip: Int!, $take: Int!, $projectId: ID!, $order:[ClIssueSortInput!], $where:ClIssueFilterInput) {
    issueList(
      skip: $skip
      take: $take
      projectId: $projectId
      order: $order,
      where: $where ) {
      pageInfo {
        hasNextPage
        hasPreviousPage
      }
      items {
        id
        issueNumber
        title
        description
        isFinalized
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
      totalCount
    }
  }`

export interface UserQueryInput {
  skip: number,
  take: number,
  order: ClUserSortInput
}

export const UserQuery = gql`
  query usersQuery($skip: Int!, $take: Int!, $order:[ClUserSortInput!], $where:ClUserFilterInput) {
    userList(
      skip: $skip
      take: $take
      order: $order,
      where: $where ) {
      pageInfo {
        hasNextPage
        hasPreviousPage
      }
      items {
        id
        name
        loginName
        email
        claims {
          type
          value
        }
      }
    }
  }
`

export const ProjectTemplateQuery = gql`
  query projectTemplateQuery {
    projectTemplateList {
      name
      description
    }
  }
`
