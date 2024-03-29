import {Injectable} from '@angular/core'
import {MatDialog, MatDialogConfig} from '@angular/material/dialog'
import {Store} from '@ngrx/store'
import {Observable} from 'rxjs'
import {ClComponent, ClIssue, ClIssueType, ClUser, ClWorkflowState} from '../Data/graphQlTypes'
import {ProjectFacade} from '../Projects/project.facade'
import {AppState} from '../Store/app.reducer'
import {getIssues, getLoadedIssues, getServerHasMoreIssues, getTotalIssues} from '../Store/app.selectors'
import {
  ConfirmDialogComponent,
  ConfirmDialogData,
  ConfirmDialogResult
} from '../UI/confirm-dialog/confirm-dialog.component'
import {ClearIssues} from './issues.actions'
import {IssueService} from './issues.service'

@Injectable( {
  providedIn: 'root'
} )
export class IssuesFacade {
  constructor( private store: Store<AppState>,
               private issueService: IssueService,
               private projectFacade: ProjectFacade,
               private dialog: MatDialog ) {
  }

  AddIssue( issue: ClIssue ) {
    this.issueService.AddIssue( issue )
  }

  ClearIssues() {
    this.store.dispatch( new ClearIssues() )
  }

  CompleteIssue( issue: ClIssue ) {
    this.issueService.CompleteIssue( issue ).then()
  }

  DeleteIssue( issue: ClIssue ) {
    const dialogData: ConfirmDialogData = {
      prompt: 'Do you want to delete this issue?',
      promptDetail: issue.title
    }
    const dialogConfig = new MatDialogConfig()
    dialogConfig.data = dialogData

    this.dialog
      .open( ConfirmDialogComponent, dialogConfig )
      .afterClosed()
      .subscribe( ( result: ConfirmDialogResult ) => {
        if( result.accepted ) {
          this.issueService.DeleteIssue( issue )
        }
      } )
  }

  UpdateIssue( issue: ClIssue ) {
    this.issueService.UpdateIssue( issue )
  }

  GetCurrentIssuesList$(): Observable<ClIssue[]> {
    return this.store.select( getIssues )
  }

  GetServerHasMoreIssues$(): Observable<boolean> {
    return this.store.select( getServerHasMoreIssues )
  }

  GetTotalIssues$(): Observable<number> {
    return this.store.select( getTotalIssues )
  }

  GetLoadedIssues$(): Observable<number> {
    return this.store.select( getLoadedIssues )
  }

  async LoadIssues(): Promise<void> {
    const project = await this.projectFacade.GetCurrentProject()

    if( project !== null ) {
      this.issueService.LoadIssues( project.id )
    }
  }

  LoadMoreIssues() {
    this.issueService.LoadMoreIssues().then()
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
}
