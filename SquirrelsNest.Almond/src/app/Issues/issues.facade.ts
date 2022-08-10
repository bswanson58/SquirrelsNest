import {Injectable} from '@angular/core'
import {Store} from '@ngrx/store'
import {Observable} from 'rxjs'
import {ClComponent, ClIssue, ClIssueType, ClUser, ClWorkflowState} from '../Data/graphQlTypes'
import {ProjectFacade} from '../Projects/project.facade'
import {AppState} from '../Store/app.reducer'
import {getIssueDisplayStyle, getIssues, getServerHasMoreIssues} from '../Store/app.selectors'
import {ToggleIssueListStyle} from '../UI/ui.actions'
import {eIssueDisplayStyle} from '../UI/ui.state'
import {ClearIssues} from './issues.actions'
import {IssueService} from './issues.service'

@Injectable( {
  providedIn: 'root'
} )
export class IssuesFacade {
  constructor( private store: Store<AppState>, private issueService: IssueService, private projectFacade: ProjectFacade ) {
  }

  AddIssue( issue: ClIssue ) {
    this.issueService.AddIssue( issue )
  }

  ClearIssues() {
    this.store.dispatch( new ClearIssues() )
  }

  GetCurrentIssuesList(): Observable<ClIssue[]> {
    return this.store.select( getIssues )
  }

  GetServerHasMoreIssues(): Observable<boolean> {
    return this.store.select( getServerHasMoreIssues )
  }

  LoadIssues() {
    const project = this.projectFacade.GetCurrentProject()

    if( project !== null ) {
      this.issueService.LoadIssues( project.id )
    }
  }

  LoadMoreIssues() {
    this.issueService.LoadMoreIssues()
  }

  UpdateIssueComponent( issue: ClIssue, component: ClComponent ): void {
    this.issueService.UpdateIssueComponent( issue, component )
  }

  UpdateIssueIssueType( issue: ClIssue, issueType: ClIssueType ): void {
    this.issueService.UpdateIssueIssueType( issue, issueType )
  }

  UpdateIssueWorkflowState( issue: ClIssue, state: ClWorkflowState ): void {
    this.issueService.UpdateIssueWorkflowState( issue, state )
  }

  UpdateIssueAssignedUser( issue: ClIssue, user: ClUser ): void {
    this.issueService.UpdateIssueAssignedUser( issue, user )
  }

  ToggleIssueListStyle() {
    this.store.dispatch( new ToggleIssueListStyle() )
  }

  GetIssueListDisplayStyle(): Observable<eIssueDisplayStyle> {
    return this.store.select( getIssueDisplayStyle )
  }
}
