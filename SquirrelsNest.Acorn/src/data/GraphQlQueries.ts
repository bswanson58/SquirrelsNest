export const PROJECTS_QUERY =
    `{  allProjects(first: 10) {
          pageInfo {
            hasNextPage
            hasPreviousPage
          }
          nodes {
            name
            description
            id
          }
          edges {
            cursor
            node {
              name
            }
          }
          totalCount
        }
      }`
      