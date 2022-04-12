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

export const ISSUES_FOR_PROJECT_QUERY = `query IssuesForProjectQuery($skip: Int!, $take: Int!, $projectId: ID!) { 
  allIssuesForProject(
      skip: $skip
      take: $take
      projectId: $projectId ) {
    pageInfo {
      hasNextPage
      hasPreviousPage
    }
    items {
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
