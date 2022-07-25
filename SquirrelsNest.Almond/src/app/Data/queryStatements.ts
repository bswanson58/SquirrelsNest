import {gql} from 'apollo-angular'
import {ClIssueSortInput, ClProjectSortInput, Query} from './graphQlTypes'

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
