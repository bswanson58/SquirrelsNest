import {Injectable} from '@angular/core'
import {GraphQLErrors} from '@apollo/client/errors'
import {Store} from '@ngrx/store'
import {Apollo, QueryRef} from 'apollo-angular'
import {firstValueFrom, map, Subscription, tap} from 'rxjs'
import {GraphQlBaseService} from '../Common/graphql.base.service'
import {
  AddIssueInput, AddIssuePayload,
  ClComponent,
  ClIssue,
  ClIssueCollectionSegment,
  ClIssueType,
  ClUser,
  ClWorkflowState,
  DeleteIssueInput, DeleteIssuePayload,
  IssueUpdatePath,
  ModifyIssueInput, ModifyIssuePayload,
  Mutation,
  Query,
  UpdateIssueInput, UpdateIssuePayload,
  UpdateOperationInput
} from '../Data/graphQlTypes'
import {AddIssueMutation, DeleteIssueMutation, ModifyIssueMutation, UpdateIssueMutation} from '../Data/issueMutations'
import {IssueQueryInput, IssuesQuery} from '../Data/queryStatements'
import {ProjectFacade} from '../Projects/project.facade'
import {AppState} from '../Store/app.reducer'
import {getIssueQueryState} from '../Store/app.selectors'
import {ReportError, ServiceCallEnded, ServiceCallStarted} from '../UI/ui.actions'
import {
  AddIssue,
  AppendIssues,
  ClearIssues,
  DeleteIssue,
  UpdateIssue
} from './issues.actions'

@Injectable( {
  providedIn: 'root'
} )
export class IssueService extends GraphQlBaseService {
  private readonly mPageLimit = 10
  private mIssueQuery: QueryRef<Query, IssueQueryInput> | null
  private mIssuesSubscription: Subscription | null

  constructor( store: Store<AppState>, private apollo: Apollo, private projectFacade: ProjectFacade ) {
    super( store )
    this.mIssuesSubscription = null
    this.mIssueQuery = null
  }

  LoadIssues( forProject: string ): void {
    this.unsubscribe()
    this.store.dispatch( new ServiceCallStarted( 'Loading Initial Issues' ) )
    this.store.dispatch( new ClearIssues() )

    this.mIssueQuery = this.apollo.use( 'issuesWatchClient' ).watchQuery<Query, IssueQueryInput>(
      {
        fetchPolicy: 'network-only',
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
        map( result => this.handleQueryErrors( result.data, result.errors ) ),
        map( result => this.handleIssueData( result ) ),
        tap( () => this.store.dispatch( new ServiceCallEnded() ) )
      )
      .subscribe( this.getServiceObserver() )
  }

  async LoadMoreIssues(): Promise<void> {
    const queryState = await firstValueFrom( this.store.select( getIssueQueryState ) )

    if( (this.mIssueQuery != null) &&
      (queryState.hasNextPage) ) {
      this.store.dispatch( new ServiceCallStarted( 'Loading Additional Issues' ) )

      this.mIssueQuery.fetchMore( {
        variables: {
          skip: queryState.loadedIssues
        }
      } ).then()
    }
  }

  private handleQueryErrors( data: Query | null, errors: GraphQLErrors | undefined ): ClIssueCollectionSegment {
    if( Array.isArray( errors ) ) {
      if( errors.length > 0 ) {
        this.store.dispatch( new ReportError( errors[0].message ) )
      }
      else {
        this.store.dispatch( new ReportError( 'Unknown error occurred' ) )
      }

      return { items: [], pageInfo: { hasNextPage: false, hasPreviousPage: false }, totalCount: 0 }
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

  async CompleteIssue( issue: ClIssue ): Promise<void> {
    const currentProject = await this.projectFacade.GetCurrentProject()
    const completedState = currentProject?.workflowStates.find( s => s.category === 'COMPLETED' )

    if( completedState !== undefined ) {
      const operation: UpdateOperationInput = {
        path: 'WORKFLOW_STATE_ID' as IssueUpdatePath.WorkflowStateId,
        value: completedState.id
      }

      this.updateIssue( { issueId: issue.id, operations: [operation] } as UpdateIssueInput )
    }
  }

  UpdateIssue( issue: ClIssue ) {
    const modifyInput: ModifyIssueInput = {
      issueId: issue.id,
      title: issue.title,
      description: issue.description,
      issueTypeId: issue.issueType?.id ?? '',
      workflowStateId: issue.workflowState?.id ?? '',
      componentId: issue.component?.id ?? '',
      releaseId: issue.release?.id ?? '',
      assignedToId: issue.assignedTo?.id ?? ''
    }

    this.apollo.use( 'defaultClient' ).mutate<Mutation>( {
      mutation: ModifyIssueMutation,
      variables: { modifyInput: modifyInput }
    } )
      .pipe(
        map( result => this.handleMutationErrors( result.data?.modifyIssue, result.errors ) ),
        map( result => {
          const payload = result as ModifyIssuePayload

          if( payload.issue != null ) {
            this.store.dispatch( new UpdateIssue( payload.issue ) )
          }

          return payload.issue
        } ),
      )
      .subscribe( this.getServiceObserver() )
  }

  private updateIssue( input: UpdateIssueInput ) {
    this.store.dispatch( new ServiceCallStarted( 'Updating Issue' ) )

    this.apollo.use( 'defaultClient' ).mutate<Mutation>( {
      mutation: UpdateIssueMutation,
      variables: { updateInput: input }
    } )
      .pipe(
        map( result => this.handleMutationErrors( result.data?.updateIssue, result.errors ) ),
        map( result => {
          const payload = result as UpdateIssuePayload

          if( payload.issue != null ) {
            this.store.dispatch( new UpdateIssue( payload.issue ) )
          }

          return payload.issue
        } ),
      )
      .subscribe( this.getServiceObserver() )
  }

  ngOnDestroy() {
    this.unsubscribe()
  }

  AddIssue( issue: ClIssue ) {
    const input: AddIssueInput = {
      title: issue.title,
      description: issue.description,
      issueTypeId: issue.issueType?.id ?? '',
      componentId: issue.component?.id ?? '',
      workflowId: issue.workflowState?.id ?? '',
      projectId: issue.project.id
    }

    this.store.dispatch( new ServiceCallStarted( 'Adding Issue' ) )

    this.apollo.use( 'defaultClient' ).mutate<Mutation>( {
      mutation: AddIssueMutation,
      variables: { issueInput: input }
    } )
      .pipe(
        map( result => this.handleMutationErrors( result.data?.addIssue, result.errors ) ),
        map( result => {
          const payload = result as AddIssuePayload

          if( payload.issue != null ) {
            this.store.dispatch( new AddIssue( payload.issue ) )
          }

          return payload.issue
        } ),
      )
      .subscribe( this.getServiceObserver() )
  }

  DeleteIssue( issue: ClIssue ) {
    const data: DeleteIssueInput = {
      issueId: issue.id
    }

    this.store.dispatch( new ServiceCallStarted( 'Deleting Issue' ) )

    this.apollo.use( 'defaultClient' ).mutate<Mutation>( {
      mutation: DeleteIssueMutation,
      variables: { deleteInput: data }
    } )
      .pipe(
        map( result => this.handleMutationErrors( result.data?.deleteIssue, result.errors ) ),
        map( result => {
          const payload = result as DeleteIssuePayload

          if( payload.issueId != null ) {
            this.store.dispatch( new DeleteIssue( payload.issueId ) )
          }

          return payload.issueId
        } ),
      )
      .subscribe( this.getServiceObserver() )
  }

  private unsubscribe() {
    if( this.mIssuesSubscription != null ) {
      this.mIssuesSubscription.unsubscribe()
      this.mIssuesSubscription = null
    }
  }
}
