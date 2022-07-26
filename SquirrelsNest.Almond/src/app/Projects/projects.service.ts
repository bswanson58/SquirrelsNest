import {Injectable} from '@angular/core'
import {Store} from '@ngrx/store'
import {Apollo, QueryRef} from 'apollo-angular'
import {map, Observable, tap} from 'rxjs'
import {ClProject, Query} from '../Data/graphQlTypes'
import {AllProjectsQuery, ProjectQueryInput} from '../Data/queryStatements'
import {AppState} from '../Store/app.reducer'
import {ClearProjectLoading, ClearProjects, AppendProjects, SetProjectLoading} from './projects.actions'

@Injectable( {
  providedIn: 'root'
} )
export class ProjectService {
  private readonly mProjectQuery: QueryRef<Query, ProjectQueryInput>
  private readonly mPageLimit = 10
  private mProjects: Observable<ClProject[]> = new Observable<ClProject[]>()
  private mProjectListLength = 0
  private mProjectListCompleted = false

  constructor( private apollo: Apollo, private store: Store<AppState> ) {
    this.mProjectQuery = this.apollo.watchQuery<Query, ProjectQueryInput>(
      {
        query: AllProjectsQuery,
        variables: { skip: this.mProjectListLength, take: this.mPageLimit, order: {} }
      } )
  }

  LoadProjects(): void {
    this.store.dispatch( new SetProjectLoading() )
    this.store.dispatch( new ClearProjects() )

    this.mProjects = this.mProjectQuery.valueChanges.pipe(
      tap( res => {
        if( res.data.projectList?.pageInfo !== undefined ) {
          this.mProjectListCompleted = !res.data.projectList.pageInfo.hasNextPage
        }
      } ),
      map( res => {
        const projectList = res?.data?.projectList?.items

        return projectList !== undefined ? projectList! : []
      } ),
      tap( projectList => this.store.dispatch( new AppendProjects( projectList ) ) ),
      tap( res => this.mProjectListLength = res.length ),
      tap( _ => this.store.dispatch( new ClearProjectLoading() ) )
    )

    // this needs some thought:
    const subscription = this.mProjects.subscribe( () => {
      subscription.unsubscribe()
    } )
  }

  LoadMoreProjects(): void {
    if( !this.mProjectListCompleted ) {
      this.mProjectQuery.fetchMore( {
        variables: {
          skip: this.mProjectListLength
        }
      } ).then()
    }
  }
}
