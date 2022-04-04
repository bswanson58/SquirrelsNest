import { AllProjectsQueryResult, AllProjectsEdge, ClProject, PageInfo } from './GraphQlEntities'

class ProjectData {
  projects: ClProject[]
  pageInfo: PageInfo
  edges: AllProjectsEdge[]
  totalProjectCount: number

  constructor(queryResult?: AllProjectsQueryResult) {
    this.projects = []
    this.edges = []
    this.totalProjectCount = 0
    this.pageInfo = { hasNextPage: false, hasPreviousPage: false, startCursor: '', endCursor: '' }

    if (queryResult !== undefined) {
      this.projects = queryResult.allProjects.nodes
      this.pageInfo = queryResult.allProjects.pageInfo
      this.edges = queryResult.allProjects.edges
      this.totalProjectCount = queryResult.allProjects.totalCount
    }
  }
}

let noProjects = new ProjectData(undefined)

export { ProjectData, noProjects }
