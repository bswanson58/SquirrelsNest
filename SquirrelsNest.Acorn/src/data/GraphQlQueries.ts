export const PROJECTS_QUERY = `{ 
  allProjects(first: 10) {
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
        name
        description
        id
      }
      totalCount
    }
  }`

export const ISSUES_FOR_PROJECT_QUERY = `{ 
  allIssuesForProject(
      first: 10 
      projectId: "05be38d6-2751-49bf-8a48-4c1823a69f7d" ) {
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
    }
    totalCount
  }
}`
