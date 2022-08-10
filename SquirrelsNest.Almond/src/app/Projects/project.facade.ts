import {Injectable} from '@angular/core'
import {Store} from '@ngrx/store'
import {map, Observable, take} from 'rxjs'
import {ClComponent, ClIssueType, ClProject, ClUser, ClWorkflowState} from '../Data/graphQlTypes'
import {AppState} from '../Store/app.reducer'
import {getSelectedProject} from '../Store/app.selectors'
import {ProjectService} from './projects.service'

@Injectable( {
  providedIn: 'root'
} )
export class ProjectFacade {
  constructor( private store: Store<AppState>, private service: ProjectService ) {
  }

  GetCurrentProject$(): Observable<ClProject | null> {
    return this.store.select( getSelectedProject )
  }

  GetCurrentProject(): ClProject | null {
    let currentProject: ClProject | null = null

    this.store.select( getSelectedProject ).pipe( take( 1 ) ).subscribe( project => currentProject = project )

    return currentProject
  }

  GetCurrentProjectComponents$(): Observable<ClComponent[]> {
    return this.store.select( getSelectedProject )
      .pipe(
        map( project => project ? project.components : [] )
      )
  }

  GetCurrentProjectIssueTypes$(): Observable<ClIssueType[]> {
    return this.store.select( getSelectedProject )
      .pipe(
        map( project => project ? project.issueTypes : [] )
      )
  }

  GetCurrentProjectWorkflowStates$(): Observable<ClWorkflowState[]> {
    return this.store.select( getSelectedProject )
      .pipe(
        map( project => project ? project.workflowStates : [] )
      )
  }

  GetCurrentProjectUsers$(): Observable<ClUser[]> {
    return this.store.select( getSelectedProject )
      .pipe(
        map( project => project ? project.users : [] )
      )
  }
}
