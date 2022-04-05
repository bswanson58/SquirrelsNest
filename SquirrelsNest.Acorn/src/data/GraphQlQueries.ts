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

export const ISSUES_FOR_PROJECT_QUERY = `query IssuesForProjectQuery($first: Int!, $projectId: ID!) { 
  allIssuesForProject(
      first: $first
      projectId: $projectId ) {
    pageInfo {
      hasNextPage
      hasPreviousPage
      startCursor
      endCursor
    }
    edges {
      cursor
      node {
        issueNumber
      }
    }
    nodes {
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
    totalCount
  }
}`
