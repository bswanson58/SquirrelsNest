import {Injectable} from '@angular/core'
import {GraphQLErrors} from '@apollo/client/errors'
import {Store} from '@ngrx/store'
import {Apollo, QueryRef} from 'apollo-angular'
import {map, Subscription, take, tap} from 'rxjs'
import {ClIssueCollectionSegment, Query} from '../Data/graphQlTypes'
import {IssuesQuery, IssueQueryInput} from '../Data/queryStatements'
import {AppState} from '../Store/app.reducer'
import {AppendIssues, ClearIssues, ClearIssuesLoading, SetIssuesLoading} from './issues.actions'
import {IssueQueryInfo} from './issues.state'
import {getIssueQueryState} from '../Store/app.selectors'

@Injectable( {
  providedIn: 'root'
} )
export class IssueService {
  private readonly mPageLimit = 10
  private mIssueQuery: QueryRef<Query, IssueQueryInput> | null
  private mIssuesSubscription: Subscription | null

  constructor( private apollo: Apollo, private store: Store<AppState> ) {
    this.mIssuesSubscription = null
    this.mIssueQuery = null
  }

  LoadIssues( forProject: string ): void {
    this.unsubscribe()
    this.store.dispatch( new SetIssuesLoading() )
    this.store.dispatch( new ClearIssues() )

    this.mIssueQuery = this.apollo.watchQuery<Query, IssueQueryInput>(
      {
        query: IssuesQuery,
        variables: { skip: 0, take: this.mPageLimit, order: {}, projectId: forProject }
      } )

    this.mIssuesSubscription = this.mIssueQuery
      .valueChanges
      .pipe(
        map( result => this.handleErrors( result.data, result.errors ) ),
        map( result => this.handleIssueData( result ) ),
        tap( _ => this.store.dispatch( new ClearIssuesLoading() ) )
      )
      .subscribe()
  }

  LoadMoreIssues(): void {
    const queryState = this.getIssueQueryState()

    if( (this.mIssueQuery != null) &&
      (queryState.hasNextPage) ) {
      this.store.dispatch( new SetIssuesLoading() )

      this.mIssueQuery.fetchMore( {
        variables: {
          skip: queryState.loadedIssues
        }
      } ).then()
    }
  }

  handleErrors( data: Query | null, errors: GraphQLErrors | undefined ): ClIssueCollectionSegment {
    if( errors != null ) {
      console.log( errors.entries() )
    }

    if( (data?.issueList?.items != null) &&
      (data.issueList.pageInfo != null) ) {
      return data.issueList
    }
    else {
      return { items: [], pageInfo: { hasNextPage: false, hasPreviousPage: false }, totalCount: 0 }
    }
  }

  handleIssueData( issueData: ClIssueCollectionSegment ): void {
    this.store.dispatch(
      new AppendIssues( issueData.items!, {
        hasNextPage: issueData.pageInfo.hasNextPage,
        hasPreviousPage: issueData.pageInfo.hasPreviousPage,
        totalIssues: issueData.totalCount,
        loadedIssues: 0 // set by the reducer.
      } ) )
  }

  getIssueQueryState(): IssueQueryInfo {
    let queryState: IssueQueryInfo

    this.store.select( getIssueQueryState ).pipe( take( 1 ) ).subscribe( state => queryState = state )

    return queryState!
  }

  ngOnDestroy() {
    this.unsubscribe()
  }

  unsubscribe() {
    if( this.mIssuesSubscription != null ) {
      this.mIssuesSubscription.unsubscribe()
      this.mIssuesSubscription = null
    }
  }
}
