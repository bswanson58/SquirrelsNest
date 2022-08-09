import {Injectable} from '@angular/core'
import {GraphQLErrors} from '@apollo/client/errors'
import {Store} from '@ngrx/store'
import {Apollo, QueryRef} from 'apollo-angular'
import {map, Subscription, take, tap} from 'rxjs'
import {
  ClIssue,
  ClIssueCollectionSegment, IssueUpdatePath,
  Mutation,
  Query,
  UpdateIssueInput
} from '../Data/graphQlTypes'
import {UpdateIssueMutation} from '../Data/mutationStatements'
import {IssueQueryInput, IssuesQuery} from '../Data/queryStatements'
import {AppState} from '../Store/app.reducer'
import {getIssueQueryState} from '../Store/app.selectors'
import {AppendIssues, ClearIssues, ClearIssuesLoading, SetIssuesLoading, UpdateIssue} from './issues.actions'
import {IssueQueryInfo} from './issues.state'

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

    this.mIssueQuery = this.apollo.use( 'issuesWatchClient' ).watchQuery<Query, IssueQueryInput>(
      {
        query: IssuesQuery,
        variables: { skip: 0, take: this.mPageLimit, order: {}, projectId: forProject }
      } )

    this.mIssuesSubscription = this.mIssueQuery
      .valueChanges
      .pipe(
        map( result => this.handleQueryErrors( result.data, result.errors ) ),
        map( result => this.handleIssueData( result ) ),
        tap( _ => this.store.dispatch( new ClearIssuesLoading() ) )
      )
      .subscribe( { complete: () => console.log( 'LoadIssues completed.' ) } )
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

  UpdateIssueIssueType( issue: ClIssue ) {
    const input: UpdateIssueInput = {
      issueId: issue.id,
      operations: [{ path: 'ISSUE_TYPE_ID' as IssueUpdatePath.IssueTypeId, value: issue.issueType.id }]
    }

    this.updateIssue( input )
  }

  UpdateIssueComponent( issue: ClIssue ) {
    const input: UpdateIssueInput = {
      issueId: issue.id,
      operations: [{ path: 'COMPONENT_ID' as IssueUpdatePath.ComponentId, value: issue.component.id }]
    }

    this.updateIssue( input )
  }

  UpdateWorkflowState( issue: ClIssue ) {
    const input: UpdateIssueInput = {
      issueId: issue.id,
      operations: [{ path: 'WORKFLOW_STATE_ID' as IssueUpdatePath.WorkflowStateId, value: issue.workflowState.id }]
    }

    this.updateIssue( input )
  }

  UpdateAssignedUser( issue: ClIssue ) {
    const input: UpdateIssueInput = {
      issueId: issue.id,
      operations: [{ path: 'ASSIGNED_TO_ID' as IssueUpdatePath.AssignedToId, value: issue.assignedTo.id }]
    }

    this.updateIssue( input )
  }

  private updateIssue( input: UpdateIssueInput ) {
    this.store.dispatch( new SetIssuesLoading() )

    this.apollo.use( 'defaultClient' ).mutate<Mutation>( {
      mutation: UpdateIssueMutation,
      variables: { updateInput: input }
    } )
      .pipe(
        map( result => IssueService.handleMutationErrors( result.data, result.errors ) ),
        map( result => {
          if( result !== null ) {
            this.store.dispatch( new UpdateIssue( result ) )
          }

          return result
        } ),
        tap( _ => this.store.dispatch( new ClearIssuesLoading() ) ),
      )
      .subscribe()
  }

  private static handleMutationErrors( data: Mutation | undefined | null, errors: GraphQLErrors | undefined ): ClIssue | null {
    if( errors != null ) {
      console.log( errors.entries() )
    }

    if( (data?.updateIssue?.errors !== undefined) &&
      (data.updateIssue.issue !== undefined) ) {
      return data.updateIssue.issue
    }

    return null
  }

  handleQueryErrors( data: Query | null, errors: GraphQLErrors | undefined ): ClIssueCollectionSegment {
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
