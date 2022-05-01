import {gql} from 'graphql-request'

export const AllProjectsQuery = gql`
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
      totalCount
    }
  }`
