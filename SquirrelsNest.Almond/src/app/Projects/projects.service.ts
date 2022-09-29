import {Injectable, OnDestroy} from '@angular/core'
import {GraphQLErrors} from '@apollo/client/errors'
import {Store} from '@ngrx/store'
import {Apollo, QueryRef} from 'apollo-angular'
import {map, Subscription, take, tap} from 'rxjs'
import {GraphQlBaseService} from '../Common/graphql.base.service'
import {
  AddProjectInput,
  AddProjectPayload,
  ClProject,
  ClProjectCollectionSegment,
  ClProjectTemplate,
  CreateTemplateInput, CreateTemplatePayload,
  DeleteProjectInput,
  DeleteProjectPayload,
  Mutation,
  Query,
  UpdateProjectInput,
  UpdateProjectPayload
} from '../Data/graphQlTypes'
import {
  AddProjectMutation,
  CreateProjectTemplate,
  DeleteProjectMutation,
  UpdateProjectMutation
} from '../Data/projectMutations'
import {AllProjectsQuery, ProjectQueryInput, ProjectTemplateQuery} from '../Data/queryStatements'
import {AppState} from '../Store/app.reducer'
import {getProjectQueryState} from '../Store/app.selectors'
import {ReportError, ServiceCallEnded, ServiceCallStarted} from '../UI/ui.actions'
import {ProjectQueryInfo} from './project.state'
import {
  ClearProjects,
  AppendProjects,
  AddProject,
  DeleteProject, UpdateProject, UpdateTemplates
} from './projects.actions'

@Injectable( {
  providedIn: 'root'
} )
export class ProjectService extends GraphQlBaseService implements OnDestroy {
  private readonly mProjectQuery: QueryRef<Query, ProjectQueryInput>
  private readonly mPageLimit = 10
  private mProjectsSubscription: Subscription | null

  constructor( private apollo: Apollo, store: Store<AppState> ) {
    super( store )
    this.mProjectsSubscription = null

    this.mProjectQuery = this.apollo.use( 'projectsWatchClient' ).watchQuery<Query, ProjectQueryInput>(
      {
        fetchPolicy: 'no-cache',
        query: AllProjectsQuery,
        variables: { skip: 0, take: this.mPageLimit, order: { name: 'ASC' } } as ProjectQueryInput
      } )
  }

  LoadProjects(): void {
    this.unsubscribe()
    this.store.dispatch( new ServiceCallStarted( 'Loading Initial Projects' ) )
    this.store.dispatch( new ClearProjects() )

    this.mProjectsSubscription = this.mProjectQuery
      .valueChanges
      .pipe(
        map( result => this.handleErrors( result.data, result.errors ) ),
        map( result => this.handleProjectData( result ) ),
        tap( _ => this.store.dispatch( new ServiceCallEnded() ) )
      )
      .subscribe( { complete: () => console.log( 'LoadProjects completed.' ) } )
  }

  private handleErrors( data: Query | null, errors: GraphQLErrors | undefined ): ClProjectCollectionSegment {
    if( Array.isArray( errors ) ) {
      if( errors.length > 0 ) {
        this.store.dispatch( new ReportError( errors[0].message ) )
      }
      else {
        this.store.dispatch( new ReportError( 'Unknown error occurred' ) )
      }

      return { items: [], pageInfo: { hasNextPage: false, hasPreviousPage: false }, totalCount: 0 }
    }

    if( (data?.projectList?.items != null) &&
      (data.projectList.pageInfo != null) ) {
      return data.projectList
    }
    else {
      return { items: [], pageInfo: { hasNextPage: false, hasPreviousPage: false }, totalCount: 0 }
    }
  }

  private handleProjectData( projectData: ClProjectCollectionSegment ): void {
    this.store.dispatch(
      new AppendProjects( projectData.items!, {
        hasNextPage: projectData.pageInfo.hasNextPage,
        hasPreviousPage: projectData.pageInfo.hasPreviousPage,
        totalProjects: projectData.totalCount,
        loadedProjects: 0 // set by the reducer.
      } ) )
  }

  LoadMoreProjects(): void {
    const queryState = this.getProjectQueryState()

    if( queryState.hasNextPage ) {
      this.store.dispatch( new ServiceCallStarted( 'Loading Additional Projects' ) )

      this.mProjectQuery.fetchMore( {
        variables: {
          skip: queryState.loadedProjects
        }
      } ).then()
    }
  }

  private getProjectQueryState(): ProjectQueryInfo {
    let queryState: ProjectQueryInfo

    this.store.select( getProjectQueryState ).pipe( take( 1 ) ).subscribe( state => queryState = state )

    return queryState!
  }

  AddProject( project: AddProjectInput ) {
    this.store.dispatch( new ServiceCallStarted( 'Adding Project' ) )

    this.apollo.use( 'defaultClient' ).mutate<Mutation>( {
      mutation: AddProjectMutation,
      variables: { addInput: project }
    } )
      .pipe(
        map( result => this.handleMutationErrors( result.data?.addProject, result.errors ) ),
        map( result => {
          const payload = result as AddProjectPayload

          if( payload?.project != null ) {
            this.store.dispatch( new AddProject( payload.project ) )
          }

          return payload.project
        } ),
        tap( _ => this.store.dispatch( new ServiceCallEnded() ) ),
      )
      .subscribe()
  }

  UpdateProject( project: UpdateProjectInput ) {
    this.store.dispatch( new ServiceCallStarted( 'Updating Project' ) )

    this.apollo.use( 'defaultClient' ).mutate<Mutation>( {
      mutation: UpdateProjectMutation,
      variables: { updateInput: project }
    } )
      .pipe(
        map( result => this.handleMutationErrors( result.data?.updateProject, result.errors ) ),
        map( result => {
          const payload = result as UpdateProjectPayload

          if( payload?.project != null ) {
            this.store.dispatch( new UpdateProject( payload.project ) )
          }

          return payload.project
        } ),
        tap( _ => this.store.dispatch( new ServiceCallEnded() ) ),
      )
      .subscribe()
  }

  DeleteProject( project: ClProject ) {
    this.store.dispatch( new ServiceCallStarted( 'Deleting Project' ) )
    const deleteInput: DeleteProjectInput = {
      projectId: project.id
    }

    this.apollo.use( 'defaultClient' ).mutate<Mutation>( {
      mutation: DeleteProjectMutation,
      variables: { deleteInput: deleteInput }
    } )
      .pipe(
        map( result => this.handleMutationErrors( result.data?.deleteProject, result.errors ) ),
        map( result => {
          const payload = result as DeleteProjectPayload

          if( payload?.projectId != null ) {
            this.store.dispatch( new DeleteProject( payload.projectId ) )
          }

          return payload.projectId
        } ),
        tap( _ => this.store.dispatch( new ServiceCallEnded() ) ),
      )
      .subscribe()
  }

  LoadProjectTemplates() {
    this.store.dispatch( new ServiceCallStarted( 'Loading Project Templates' ) )

    this.apollo.use( 'defaultClient' ).query<Query>( { query: ProjectTemplateQuery } )
      .pipe(
        map( result => ProjectService.handleTemplateQueryErrors( result.data, result.errors ) ),
        map( result => {
          if( result !== null ) {
            this.store.dispatch( new UpdateTemplates( result ) )
          }
        } ),
        tap( _ => this.store.dispatch( new ServiceCallEnded() ) ),
      )
      .subscribe()
  }

  private static handleTemplateQueryErrors( data: Query | undefined | null, errors: GraphQLErrors | undefined ): ClProjectTemplate[] {
    if( errors != null ) {
      console.log( errors.entries() )
    }

    return data?.projectTemplateList ?? []
  }

  CreateProjectTemplate( templateInput: CreateTemplateInput ) {
    this.store.dispatch( new ServiceCallStarted( 'Creating Project Template' ) )

    this.apollo.use( 'defaultClient' ).mutate<Mutation>( {
      mutation: CreateProjectTemplate,
      variables: { templateInput: templateInput }
    } )
      .pipe(
        map( result => this.handleMutationErrors( result.data?.createProjectTemplate, result.errors ) ),
        tap( result => {
          const payload = result as CreateTemplatePayload

          if( !payload.succeeded ) {
            this.store.dispatch( new ReportError( 'Creating the project template failed' ) )
          }
        } ),
        tap( _ => this.store.dispatch( new ServiceCallEnded() ) ),
      )
      .subscribe()
  }

  ngOnDestroy() {
    this.unsubscribe()
  }

  unsubscribe() {
    if( this.mProjectsSubscription != null ) {
      this.mProjectsSubscription.unsubscribe()
      this.mProjectsSubscription = null
    }

    this.mProjectQuery.resetLastResults()
  }
}
