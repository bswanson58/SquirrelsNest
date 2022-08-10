import {Injectable} from '@angular/core'
import {Store} from '@ngrx/store'
import {map, Observable, take} from 'rxjs'
import {ClComponent, ClIssueType, ClProject, ClUser, ClWorkflowState} from '../Data/graphQlTypes'
import {ClearIssues} from '../Issues/issues.actions'
import {AppState} from '../Store/app.reducer'
import {getProjects, getSelectedProject, getServerHasMoreProjects} from '../Store/app.selectors'
import {SelectProject} from './projects.actions'
import {ProjectService} from './projects.service'

@Injectable( {
  providedIn: 'root'
} )
export class ProjectFacade {
  constructor( private store: Store<AppState>, private projectService: ProjectService ) {
  }

  GetProjectList$(): Observable<ClProject[]> {
    return this.store.select( getProjects )
  }

  GetServerHasMoreProjects(): Observable<boolean> {
    return this.store.select( getServerHasMoreProjects )
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

  LoadProjects(): void {
    this.projectService.LoadProjects()
  }

  LoadMoreProjects(): void {
    this.projectService.LoadMoreProjects()
  }

  SelectProject( project: ClProject ): void {
    this.store.dispatch( new ClearIssues() )
    this.store.dispatch( new SelectProject( project ) )
  }
}
