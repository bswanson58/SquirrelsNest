import { ClIssue, PageInfo, AllIssuesForProjectEdge, AllIssuesForProjectQueryResult } from './GraphQlEntities'

class IssueData {
    issues: ClIssue[]
    pageInfo: PageInfo
    edges: AllIssuesForProjectEdge[]
    totalCount: number
     
    constructor(queryResult?: AllIssuesForProjectQueryResult) {
        this.issues = []
        this.edges = []
        this.totalCount = 0
        this.pageInfo = { hasNextPage: false, hasPreviousPage: false, startCursor: '', endCursor: '' }

        if(queryResult !== undefined) {
            this.issues = queryResult.allIssuesForProject.nodes
            this.edges = queryResult.allIssuesForProject.edges
            this.pageInfo = queryResult.allIssuesForProject.pageInfo
            this.totalCount = queryResult.allIssuesForProject.totalCount
         }
    }
}

let noIssues = new IssueData(undefined)

export { IssueData, noIssues }