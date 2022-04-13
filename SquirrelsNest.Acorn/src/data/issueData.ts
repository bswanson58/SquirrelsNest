import {ClIssue, AllIssuesForProjectQueryResult, CollectionSegmentInfo} from './GraphQlEntities'
/*
class IssueData {
  issues: ClIssue[]
  pageInfo: PageInfo
  totalCount: number

  constructor( queryResult?: AllIssuesForProjectQueryResult ) {
    this.issues = []
    this.totalCount = 0
    this.pageInfo = { hasNextPage: false, hasPreviousPage: false, startCursor: '', endCursor: '' }

    if( queryResult !== undefined ) {
      this.issues = queryResult.allIssuesForProject.items
      this.pageInfo = queryResult.allIssuesForProject.pageInfo
      this.totalCount = queryResult.allIssuesForProject.totalCount
    }
  }
}
*/
function initializeIssueData() {
  let issues: ClIssue[] = []
  let totalCount: number = 0
  let pageInfo: CollectionSegmentInfo = { hasNextPage: false, hasPreviousPage: false }

  function updateIssueData( data: AllIssuesForProjectQueryResult ) {
    issues = [...issues, ...data.allIssuesForProject.items]
    totalCount = data.allIssuesForProject.totalCount
    pageInfo = data.allIssuesForProject.pageInfo
  }

  function clearIssueData() {
    totalCount = 0;
    pageInfo = { hasNextPage: false, hasPreviousPage: false }
    issues = []
  }

  return { issues, totalCount, pageInfo, clearIssueData, updateIssueData }
}
export {initializeIssueData}
