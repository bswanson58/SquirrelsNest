import {Injectable, OnDestroy} from '@angular/core'
import {GraphQLErrors} from '@apollo/client/errors'
import {Store} from '@ngrx/store'
import {Apollo, QueryRef} from 'apollo-angular'
import {map, Subscription, take, tap} from 'rxjs'
import {
  AddProjectInput,
  ClProject,
  ClProjectCollectionSegment, ClProjectTemplate, CreateTemplateInput, DeleteProjectInput, Mutation,
  Query,
  UpdateProjectInput
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
import {ProjectQueryInfo} from './project.state'
import {
  ClearProjectLoading,
  ClearProjects,
  AppendProjects,
  SetProjectLoading,
  AddProject,
  DeleteProject, UpdateProject, UpdateTemplates
} from './projects.actions'

@Injectable( {
  providedIn: 'root'
} )
export class ProjectService implements OnDestroy {
  private readonly mProjectQuery: QueryRef<Query, ProjectQueryInput>
  private readonly mPageLimit = 10
  private mProjectsSubscription: Subscription | null

  constructor( private apollo: Apollo, private store: Store<AppState> ) {
    this.mProjectsSubscription = null

    this.mProjectQuery = this.apollo.use( 'projectsWatchClient' ).watchQuery<Query, ProjectQueryInput>(
      {
        query: AllProjectsQuery,
        variables: { skip: 0, take: this.mPageLimit, order: { name: 'ASC' } } as ProjectQueryInput
      } )
  }

  LoadProjects(): void {
    this.unsubscribe()
    this.store.dispatch( new SetProjectLoading() )
    this.store.dispatch( new ClearProjects() )

    this.mProjectsSubscription = this.mProjectQuery
      .valueChanges
      .pipe(
        map( result => this.handleErrors( result.data, result.errors ) ),
        map( result => this.handleProjectData( result ) ),
        tap( _ => this.store.dispatch( new ClearProjectLoading() ) )
      )
      .subscribe( { complete: () => console.log( 'LoadProjects completed.' ) } )
  }

  handleErrors( data: Query | null, errors: GraphQLErrors | undefined ): ClProjectCollectionSegment {
    if( errors != null ) {
      console.log( errors.entries() )
    }

    if( (data?.projectList?.items != null) &&
      (data.projectList.pageInfo != null) ) {
      return data.projectList
    }
    else {
      return { items: [], pageInfo: { hasNextPage: false, hasPreviousPage: false }, totalCount: 0 }
    }
  }

  handleProjectData( projectData: ClProjectCollectionSegment ): void {
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
      this.store.dispatch( new SetProjectLoading() )

      this.mProjectQuery.fetchMore( {
        variables: {
          skip: queryState.loadedProjects
        }
      } ).then()
    }
  }

  getProjectQueryState(): ProjectQueryInfo {
    let queryState: ProjectQueryInfo

    this.store.select( getProjectQueryState ).pipe( take( 1 ) ).subscribe( state => queryState = state )

    return queryState!
  }

  AddProject( project: AddProjectInput ) {
    this.store.dispatch( new SetProjectLoading() )

    this.apollo.use( 'defaultClient' ).mutate<Mutation>( {
      mutation: AddProjectMutation,
      variables: { addInput: project }
    } )
      .pipe(
        map( result => ProjectService.handleAddMutationErrors( result.data, result.errors ) ),
        map( result => {
          if( result !== null ) {
            this.store.dispatch( new AddProject( result ) )
          }

          return result
        } ),
        tap( _ => this.store.dispatch( new ClearProjectLoading() ) ),
      )
      .subscribe()
  }

  private static handleAddMutationErrors( data: Mutation | undefined | null, errors: GraphQLErrors | undefined ): ClProject | null {
    if( errors != null ) {
      console.log( errors.entries() )
    }

    if( (data?.addProject?.errors !== undefined) &&
      (data.addProject.project !== undefined) ) {
      return data.addProject.project
    }

    return null
  }

  UpdateProject( project: UpdateProjectInput ) {
    this.store.dispatch( new SetProjectLoading() )

    this.apollo.use( 'defaultClient' ).mutate<Mutation>( {
      mutation: UpdateProjectMutation,
      variables: { updateInput: project }
    } )
      .pipe(
        map( result => ProjectService.handleUpdateMutationErrors( result.data, result.errors ) ),
        map( result => {
          if( result !== null ) {
            this.store.dispatch( new UpdateProject( result ) )
          }

          return result
        } ),
        tap( _ => this.store.dispatch( new ClearProjectLoading() ) ),
      )
      .subscribe()
  }

  private static handleUpdateMutationErrors( data: Mutation | undefined | null, errors: GraphQLErrors | undefined ): ClProject | null {
    if( errors != null ) {
      console.log( errors.entries() )
    }

    if( (data?.updateProject?.errors !== undefined) &&
      (data.updateProject.project !== undefined) ) {
      return data.updateProject.project
    }

    return null
  }

  DeleteProject( project: ClProject ) {
    this.store.dispatch( new SetProjectLoading() )
    const deleteInput: DeleteProjectInput = {
      projectId: project.id
    }

    this.apollo.use( 'defaultClient' ).mutate<Mutation>( {
      mutation: DeleteProjectMutation,
      variables: { deleteInput: deleteInput }
    } )
      .pipe(
        map( result => ProjectService.handleDeleteMutationErrors( result.data, result.errors ) ),
        map( result => {
          if( result !== null ) {
            this.store.dispatch( new DeleteProject( result ) )
          }

          return result
        } ),
        tap( _ => this.store.dispatch( new ClearProjectLoading() ) ),
      )
      .subscribe()
  }

  private static handleDeleteMutationErrors( data: Mutation | undefined | null, errors: GraphQLErrors | undefined ): string | null {
    if( errors != null ) {
      console.log( errors.entries() )
    }

    if( data?.deleteProject?.errors !== undefined ) {
      return data.deleteProject.projectId
    }

    return null
  }

  LoadProjectTemplates() {
    this.store.dispatch( new SetProjectLoading() )

    this.apollo.use( 'defaultClient' ).query<Query>( { query: ProjectTemplateQuery } )
      .pipe(
        map( result => ProjectService.handleTemplateQueryErrors( result.data, result.errors ) ),
        map( result => {
          if( result !== null ) {
            this.store.dispatch( new UpdateTemplates( result ) )
          }
        } ),
        tap( _ => this.store.dispatch( new ClearProjectLoading() ) ),
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
    this.store.dispatch( new SetProjectLoading() )

    this.apollo.use( 'defaultClient' ).mutate<Mutation>( {
      mutation: CreateProjectTemplate,
      variables: { templateInput: templateInput }
    } )
      .pipe(
        map( result => ProjectService.handleTemplateMutationErrors( result.data, result.errors ) ),
        tap( _ => this.store.dispatch( new ClearProjectLoading() ) ),
      )
      .subscribe()
  }

  private static handleTemplateMutationErrors( data: Mutation | undefined | null, errors: GraphQLErrors | undefined ): boolean {
    if( errors != null ) {
      console.log( errors.entries() )
    }

    if( data?.createProjectTemplate?.errors !== undefined ) {
      return data.createProjectTemplate.succeeded
    }

    return false
  }

  ngOnDestroy() {
    this.unsubscribe()
  }

  unsubscribe() {
    if( this.mProjectsSubscription != null ) {
      this.mProjectsSubscription.unsubscribe()
      this.mProjectsSubscription = null
    }
  }
}
