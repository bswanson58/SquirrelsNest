import {Component, Input} from '@angular/core'
import {MatDialog, MatDialogConfig} from '@angular/material/dialog'
import {Observable} from 'rxjs'
import {ClIssue, ClProject} from '../../Data/graphQlTypes'
import {ProjectFacade} from '../../Projects/project.facade'
import {UiFacade} from '../../UI/ui.facade'
import {eIssueDisplayStyle} from '../../UI/ui.state'
import {
  DetailSelectorData, DetailSelectorResult,
  IssueDetailSelectorComponent
} from '../issue-detail-selector/issue-detail-selector.component'
import {IssuesFacade} from '../issues.facade'

@Component( {
  selector: 'sn-issue-detail',
  templateUrl: './issue-detail.component.html',
  styleUrls: ['./issue-detail.component.css']
} )
export class IssueDetailComponent {
  isHovering: boolean = false
  @Input() issue!: ClIssue
  readonly project: ClProject | null

  issueListStyle$: Observable<eIssueDisplayStyle>

  constructor( private dialog: MatDialog,
               private issuesFacade: IssuesFacade,
               private projectFacade: ProjectFacade,
               private uiFacade: UiFacade ) {
    this.issueListStyle$ = this.uiFacade.GetIssueListDisplayStyle()
    this.project = this.projectFacade.GetCurrentProject()
  }

  onCompleteIssue() {
    this.issuesFacade.CompleteIssue( this.issue )
  }

  onEditIssue() {
  }

  onDeleteIssue() {
  }

  onIssueType() {
    const selectorData: DetailSelectorData = {
      issueTitle: this.issue.title,
      dialogTitle: 'Issue Type:',
      currentItem: this.issue.issueType,
      items: this.project !== null ? this.project.issueTypes : [],
    }
    const dialogConfig = new MatDialogConfig()
    dialogConfig.data = selectorData

    this.dialog
      .open( IssueDetailSelectorComponent, dialogConfig )
      .afterClosed()
      .subscribe( ( result: DetailSelectorResult ) => {
        if( result.accepted ) {
          const newType = this.project?.issueTypes.find( i => i.id === result.selectedId )

          if( newType !== undefined ) {
            this.issuesFacade.UpdateIssueIssueType( this.issue, newType )
          }
        }
      } )
  }

  onComponentType() {
    const selectorData: DetailSelectorData = {
      issueTitle: this.issue.title,
      dialogTitle: 'Component Type:',
      currentItem: this.issue.component,
      items: this.project !== null ? this.project.components : [],
    }
    const dialogConfig = new MatDialogConfig()
    dialogConfig.data = selectorData

    this.dialog
      .open( IssueDetailSelectorComponent, dialogConfig )
      .afterClosed()
      .subscribe( ( result: DetailSelectorResult ) => {
        if( result.accepted ) {
          const newComponent = this.project?.components.find( c => c.id === result.selectedId )

          if( newComponent !== undefined ) {
            this.issuesFacade.UpdateIssueComponent( this.issue, newComponent )
          }
        }
      } )
  }

  onAssignedTo() {
    const selectorData: DetailSelectorData = {
      issueTitle: this.issue.title,
      dialogTitle: 'Assigned User:',
      currentItem: this.issue.assignedTo,
      items: this.project !== null ? this.project.users : [],
    }
    const dialogConfig = new MatDialogConfig()
    dialogConfig.data = selectorData

    this.dialog
      .open( IssueDetailSelectorComponent, dialogConfig )
      .afterClosed()
      .subscribe( ( result: DetailSelectorResult ) => {
        if( result.accepted ) {
          const newUser = this.project?.users.find( c => c.id === result.selectedId )

          if( newUser !== undefined ) {
            this.issuesFacade.UpdateIssueAssignedUser( this.issue, newUser )
          }
        }
      } )
  }

  onWorkflowState() {
    const selectorData: DetailSelectorData = {
      issueTitle: this.issue.title,
      dialogTitle: 'Workflow State:',
      currentItem: this.issue.workflowState,
      items: this.project !== null ? this.project.workflowStates : [],
    }
    const dialogConfig = new MatDialogConfig()
    dialogConfig.data = selectorData

    this.dialog
      .open( IssueDetailSelectorComponent, dialogConfig )
      .afterClosed()
      .subscribe( ( result: DetailSelectorResult ) => {
        if( result.accepted ) {
          const newState = this.project?.workflowStates.find( c => c.id === result.selectedId )

          if( newState !== undefined ) {
            this.issuesFacade.UpdateIssueWorkflowState( this.issue, newState )
          }
        }
      } )
  }
}
