import { ClIssue, PageInfo, AllIssuesForProjectQueryResult } from './GraphQlEntities'

class IssueData {
    issues: ClIssue[]
    pageInfo: PageInfo
    totalCount: number
     
    constructor(queryResult?: AllIssuesForProjectQueryResult) {
        this.issues = []
        this.totalCount = 0
        this.pageInfo = { hasNextPage: false, hasPreviousPage: false, startCursor: '', endCursor: '' }

        if(queryResult !== undefined) {
            this.issues = queryResult.allIssuesForProject.items
            this.pageInfo = queryResult.allIssuesForProject.pageInfo
            this.totalCount = queryResult.allIssuesForProject.totalCount
         }
    }
}

let noIssues = new IssueData(undefined)

export { IssueData, noIssues }
