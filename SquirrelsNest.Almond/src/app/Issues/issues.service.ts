import {Injectable} from '@angular/core'
import {Apollo, QueryRef} from 'apollo-angular'
import {map, Observable, tap} from 'rxjs'
import {ClIssue, Query} from '../Data/graphQlTypes'
import {IssuesQuery, IssueQueryInput} from '../Data/queryStatements'

@Injectable( {
  providedIn: 'root'
} )
export class IssueService {
  private readonly mPageLimit = 10
  private mIssueQuery: QueryRef<Query, IssueQueryInput> | null
  private mIssues: Observable<ClIssue[]> = new Observable<ClIssue[]>()
  private mIssueListLength = 0
  private mIssueListCompleted = false

  constructor( private apollo: Apollo ) {
    this.mIssueQuery = null
  }

  LoadIssues( forProject: string ): Observable<ClIssue[]> {
    this.mIssueQuery = this.apollo.watchQuery<Query, IssueQueryInput>(
      {
        query: IssuesQuery,
        variables: { skip: this.mIssueListLength, take: this.mPageLimit, order: {}, projectId: forProject }
      } )

    this.mIssues = this.mIssueQuery.valueChanges.pipe(
      tap( res => {
        if( res.data.issueList?.pageInfo !== undefined ) {
          this.mIssueListCompleted = !res.data.issueList.pageInfo.hasNextPage
        }
      } ),
      map( res => {
        const issueList = res?.data?.issueList?.items

        return issueList !== undefined ? issueList! : []
      } ),
      tap( res => this.mIssueListLength = res.length )
    )

    return this.mIssues
  }

  LoadMoreIssues(): void {
    if( (this.mIssueQuery != null) &&
      (!this.mIssueListCompleted) ) {
      this.mIssueQuery.fetchMore( {
        variables: {
          skip: this.mIssueListLength
        }
      } ).then()
    }
  }
}
