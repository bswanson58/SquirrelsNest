import {Injectable} from '@angular/core'
import {Apollo, QueryRef} from 'apollo-angular'
import {map, Observable, tap} from 'rxjs'
import {ClProject, Query} from './graphQlTypes'
import {AllProjectsQuery, ProjectQueryInput} from './queryStatements'

@Injectable( {
  providedIn: 'root'
} )
export class ProjectProviderService {
  private readonly mProjectQuery: QueryRef<Query, ProjectQueryInput>
  private readonly mPageLimit = 1
  private mProjects: Observable<ClProject[]> = new Observable<ClProject[]>()
  private mProjectListLength = 0
  private mProjectListCompleted = false;

  constructor( private apollo: Apollo ) {
    this.mProjectQuery = this.apollo.watchQuery<Query, ProjectQueryInput>(
      {
        query: AllProjectsQuery,
        variables: { skip: this.mProjectListLength, take: this.mPageLimit, order: {} }
      } )
  }

  LoadProjects(): Observable<ClProject[]> {
    this.mProjects = this.mProjectQuery.valueChanges.pipe(
      tap( res => {
        if( res.data.projectList?.pageInfo !== undefined ) {
          this.mProjectListCompleted = !res.data.projectList.pageInfo.hasNextPage
        }
      }),
      map( res => {
        const projectList = res?.data?.projectList?.items

        return projectList !== undefined ? projectList! : []
      }),
      tap( res => this.mProjectListLength = res.length )
    )

    return this.mProjects
  }

  LoadMoreProjects(): void {
    if(!this.mProjectListCompleted ) {
      this.mProjectQuery.fetchMore( {
        variables: {
          skip: this.mProjectListLength
        }
      } )
    }
  }
}
