import {gql} from 'graphql-request'

export const LoginQuery = gql`
  query login($userCredentials: UserCredentialsInput!) {
    login(userCredentials: $userCredentials) {
      token
      expiration
    }
  }`

export const AllProjectsQuery = gql`
  query ProjectsQuery($first: Int!) {
    allProjects(first: $first) {
      pageInfo {
        hasNextPage
        hasPreviousPage
      }
      edges {
        cursor
        node {
          name
        }
      }
      nodes {
        id
        name
        description
        issuePrefix
      }
      totalCount
    }
  }`

export const IssuesQuery = gql`
  query IssuesForProjectQuery($skip: Int!, $take: Int!, $projectId: ID!, $order:[ClIssueSortInput!]) { 
    allIssuesForProject(
        skip: $skip
        take: $take
        projectId: $projectId
        order: $order ) {
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

export const LOGIN_QUERY = `query login($credentials: UserCredentialsInput!) {
  login(userCredentials: $credentials) {
    token
    expiration
  }
}`

export const PROJECTS_QUERY = `query ProjectsQuery($first: Int!) { 
  allProjects(first: $first) {
      pageInfo {
        hasNextPage
        hasPreviousPage
      }
      edges {
        cursor
        node {
          name
        }
      }
      nodes {
        id
        name
        description
        issuePrefix
      }
      totalCount
    }
  }`

export const ISSUES_FOR_PROJECT_QUERY =
  `query IssuesForProjectQuery($skip: Int!, $take: Int!, $projectId: ID!, $order:[ClIssueSortInput!]) { 
  allIssuesForProject(
      skip: $skip
      take: $take
      projectId: $projectId
      order: $order ) {
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
