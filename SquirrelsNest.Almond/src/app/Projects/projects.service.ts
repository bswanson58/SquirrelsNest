import {Injectable, OnDestroy} from '@angular/core'
import {GraphQLErrors} from '@apollo/client/errors'
import {Store} from '@ngrx/store'
import {Apollo, QueryRef} from 'apollo-angular'
import {map, Subscription, take, tap} from 'rxjs'
import {ClProjectCollectionSegment, Query} from '../Data/graphQlTypes'
import {AllProjectsQuery, ProjectQueryInput} from '../Data/queryStatements'
import {AppState} from '../Store/app.reducer'
import {getProjectQueryState} from '../Store/app.selectors'
import {ProjectQueryInfo} from './project.state'
import {ClearProjectLoading, ClearProjects, AppendProjects, SetProjectLoading} from './projects.actions'

@Injectable( {
  providedIn: 'root'
} )
export class ProjectService implements OnDestroy {
  private readonly mProjectQuery: QueryRef<Query, ProjectQueryInput>
  private readonly mPageLimit = 1
  private mProjectsSubscription: Subscription | null

  constructor( private apollo: Apollo, private store: Store<AppState> ) {
    this.mProjectsSubscription = null

    this.mProjectQuery = this.apollo.watchQuery<Query, ProjectQueryInput>(
      {
        query: AllProjectsQuery,
        variables: { skip: 0, take: this.mPageLimit, order: {} }
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
      .subscribe()
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
