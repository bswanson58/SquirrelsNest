import {Injectable} from '@angular/core'
import {Store} from '@ngrx/store'
import {Observable} from 'rxjs'
import {ClComponent, ClIssue, ClIssueType, ClUser, ClWorkflowState} from '../Data/graphQlTypes'
import {AppState} from '../Store/app.reducer'
import {getIssueDisplayStyle} from '../Store/app.selectors'
import {ToggleIssueListStyle} from '../UI/ui.actions'
import {eIssueDisplayStyle} from '../UI/ui.state'
import {IssueService} from './issues.service'

@Injectable( {
  providedIn: 'root'
} )
export class IssuesFacade {
  constructor( private store: Store<AppState>, private issueService: IssueService ) {
  }

  AddIssue( issue: ClIssue ) {
    this.issueService.AddIssue( issue )
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
