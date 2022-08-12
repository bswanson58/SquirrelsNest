import {Injectable} from '@angular/core'
import {GraphQLErrors} from '@apollo/client/errors'
import {Store} from '@ngrx/store'
import {Apollo, QueryRef} from 'apollo-angular'
import {map, Subscription, take, tap} from 'rxjs'
import {
  AddIssueInput,
  ClComponent,
  ClIssue,
  ClIssueCollectionSegment,
  ClIssueType,
  ClUser,
  ClWorkflowState,
  IssueUpdatePath,
  Mutation,
  Query,
  UpdateIssueInput,
  UpdateOperationInput
} from '../Data/graphQlTypes'
import {AddIssueMutation, UpdateIssueMutation} from '../Data/mutationStatements'
import {IssueQueryInput, IssuesQuery} from '../Data/queryStatements'
import {ProjectFacade} from '../Projects/project.facade'
import {AppState} from '../Store/app.reducer'
import {getIssueQueryState} from '../Store/app.selectors'
import {AddIssue, AppendIssues, ClearIssues, ClearIssuesLoading, SetIssuesLoading, UpdateIssue} from './issues.actions'
import {IssueQueryInfo} from './issues.state'

@Injectable( {
  providedIn: 'root'
} )
export class IssueService {
  private readonly mPageLimit = 10
  private mIssueQuery: QueryRef<Query, IssueQueryInput> | null
  private mIssuesSubscription: Subscription | null

  constructor( private apollo: Apollo, private store: Store<AppState>, private projectFacade: ProjectFacade ) {
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
        variables: {
          skip: 0,
          take: this.mPageLimit,
          order: {
            // @ts-ignore - for an unknown reason, importing SortEnumType causes a module error
            isFinalized: 'ASC',
            // @ts-ignore - for an unknown reason, importing SortEnumType causes a module error
            issueNumber: 'DESC'
          },
          projectId: forProject
        } as IssueQueryInput
      } )

    this.mIssuesSubscription = this.mIssueQuery
      .valueChanges
      .pipe(
        map( result => IssueService.handleQueryErrors( result.data, result.errors ) ),
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

  UpdateIssueIssueType( issue: ClIssue, issueType: ClIssueType ) {
    const input: UpdateIssueInput = {
      issueId: issue.id,
      operations: [{ path: 'ISSUE_TYPE_ID' as IssueUpdatePath.IssueTypeId, value: issueType.id }]
    }

    this.updateIssue( input )
  }

  UpdateIssueComponent( issue: ClIssue, component: ClComponent ) {
    const input: UpdateIssueInput = {
      issueId: issue.id,
      operations: [{ path: 'COMPONENT_ID' as IssueUpdatePath.ComponentId, value: component.id }]
    }

    this.updateIssue( input )
  }

  UpdateIssueWorkflowState( issue: ClIssue, state: ClWorkflowState ) {
    const input: UpdateIssueInput = {
      issueId: issue.id,
      operations: [{ path: 'WORKFLOW_STATE_ID' as IssueUpdatePath.WorkflowStateId, value: state.id }]
    }

    this.updateIssue( input )
  }

  UpdateIssueAssignedUser( issue: ClIssue, user: ClUser ) {
    const input: UpdateIssueInput = {
      issueId: issue.id,
      operations: [{ path: 'ASSIGNED_TO_ID' as IssueUpdatePath.AssignedToId, value: user.id }]
    }

    this.updateIssue( input )
  }

  CompleteIssue( issue: ClIssue ) {
    const completedState = this.projectFacade.GetCurrentProject()?.workflowStates.find( s => s.category === 'COMPLETED' )

    if( completedState !== undefined ) {
      const operation: UpdateOperationInput = {
        path: 'WORKFLOW_STATE_ID' as IssueUpdatePath.WorkflowStateId,
        value: completedState.id
      }

      this.updateIssue({ issueId: issue.id, operations: [operation] } as UpdateIssueInput)
    }
  }


  private updateIssue( input: UpdateIssueInput ) {
    this.store.dispatch( new SetIssuesLoading() )

    this.apollo.use( 'defaultClient' ).mutate<Mutation>( {
      mutation: UpdateIssueMutation,
      variables: { updateInput: input }
    } )
      .pipe(
        map( result => IssueService.handleUpdateMutationErrors( result.data, result.errors ) ),
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

  private static handleUpdateMutationErrors( data: Mutation | undefined | null, errors: GraphQLErrors | undefined ): ClIssue | null {
    if( errors != null ) {
      console.log( errors.entries() )
    }

    if( (data?.updateIssue?.errors !== undefined) &&
      (data.updateIssue.issue !== undefined) ) {
      return data.updateIssue.issue
    }

    return null
  }

  private static handleQueryErrors( data: Query | null, errors: GraphQLErrors | undefined ): ClIssueCollectionSegment {
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

  private handleIssueData( issueData: ClIssueCollectionSegment ): void {
    this.store.dispatch(
      new AppendIssues( issueData.items!, {
        hasNextPage: issueData.pageInfo.hasNextPage,
        hasPreviousPage: issueData.pageInfo.hasPreviousPage,
        totalIssues: issueData.totalCount,
        loadedIssues: 0 // set by the reducer.
      } ) )
  }

  private getIssueQueryState(): IssueQueryInfo {
    let queryState: IssueQueryInfo

    this.store.select( getIssueQueryState ).pipe( take( 1 ) ).subscribe( state => queryState = state )

    return queryState!
  }

  ngOnDestroy() {
    this.unsubscribe()
  }

  AddIssue( issue: ClIssue ) {
    const input: AddIssueInput = {
      title: issue.title,
      description: issue.description,
      issueTypeId: issue.issueType.id,
      projectId: issue.project.id
    }

    this.store.dispatch( new SetIssuesLoading() )

    this.apollo.use( 'defaultClient' ).mutate<Mutation>( {
      mutation: AddIssueMutation,
      variables: { issueInput: input }
    } )
      .pipe(
        map( result => IssueService.handleAddMutationErrors( result.data, result.errors ) ),
        map( result => {
          if( result !== null ) {
            this.store.dispatch( new AddIssue( result ) )
          }

          return result
        } ),
        tap( _ => this.store.dispatch( new ClearIssuesLoading() ) ),
      )
      .subscribe()
  }

  private static handleAddMutationErrors( data: Mutation | undefined | null, errors: GraphQLErrors | undefined ): ClIssue | null {
    if( errors != null ) {
      console.log( errors.entries() )
    }

    if( (data?.addIssue?.errors !== undefined) &&
      (data.addIssue.issue !== undefined) ) {
      return data.addIssue.issue
    }

    return null
  }

  private unsubscribe() {
    if( this.mIssuesSubscription != null ) {
      this.mIssuesSubscription.unsubscribe()
      this.mIssuesSubscription = null
    }
  }
}
