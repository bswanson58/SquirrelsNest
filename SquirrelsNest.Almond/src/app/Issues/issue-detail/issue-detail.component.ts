import {Component, Input, OnDestroy, OnInit} from '@angular/core'
import {MatDialog, MatDialogConfig} from '@angular/material/dialog'
import {Store} from '@ngrx/store'
import {Observable, Subscription} from 'rxjs'
import {ClIssue, ClProject} from '../../Data/graphQlTypes'
import {AppState} from '../../Store/app.reducer'
import {getIssueDisplayStyle, getSelectedProject} from '../../Store/app.selectors'
import {eIssueDisplayStyle} from '../../UI/ui.state'
import {
  DetailSelectorData, DetailSelectorResult,
  IssueDetailSelectorComponent
} from '../issue-detail-selector/issue-detail-selector.component'
import {IssueService} from '../issues.service'

@Component( {
  selector: 'sn-issue-detail',
  templateUrl: './issue-detail.component.html',
  styleUrls: ['./issue-detail.component.css']
} )
export class IssueDetailComponent implements OnInit, OnDestroy {
  @Input() issue!: ClIssue
  private mSubscription: Subscription | undefined
  private mProjectSubscription: Subscription | undefined
  private mProject: ClProject | undefined

  issueListStyle$: Observable<eIssueDisplayStyle>

  constructor( private store: Store<AppState>, private dialog: MatDialog, private issueService: IssueService ) {
    this.issueListStyle$ = new Observable<eIssueDisplayStyle>()
  }

  ngOnInit(): void {
    this.issueListStyle$ = this.store.select( getIssueDisplayStyle )
    this.mProjectSubscription = this.store.select( getSelectedProject ).subscribe( project => {
      if( project != null ) {
        this.mProject = project
      }
    } )
  }

  onIssueType() {
    const selectorData: DetailSelectorData = {
      issueTitle: this.issue.title,
      dialogTitle: 'Issue Type:',
      currentItem: this.issue.issueType,
      items: this.mProject !== undefined ? this.mProject.issueTypes : [],
    }
    const dialogConfig = new MatDialogConfig()
    dialogConfig.data = selectorData

    this.mSubscription = this.dialog
      .open( IssueDetailSelectorComponent, dialogConfig )
      .afterClosed()
      .subscribe( ( result: DetailSelectorResult ) => {
        if( result.accepted ) {
          const newType = this.mProject?.issueTypes.find( i => i.id === result.selectedId )

          if( newType !== undefined ) {
            this.issueService.UpdateIssueIssueType( { ...this.issue, issueType: newType } )
          }
        }
      } )
  }

  onComponentType() {
    const selectorData: DetailSelectorData = {
      issueTitle: this.issue.title,
      dialogTitle: 'Component Type:',
      currentItem: this.issue.component,
      items: this.mProject !== undefined ? this.mProject.components : [],
    }
    const dialogConfig = new MatDialogConfig()
    dialogConfig.data = selectorData

    this.mSubscription = this.dialog
      .open( IssueDetailSelectorComponent, dialogConfig )
      .afterClosed()
      .subscribe( ( result: DetailSelectorResult ) => {
        if( result.accepted ) {
          const newComponent = this.mProject?.components.find( c => c.id === result.selectedId )

          if( newComponent !== undefined ) {
            this.issueService.UpdateIssueComponent( { ...this.issue, component: newComponent } )
          }
        }
      } )
  }

  onAssignedTo() {
    const selectorData: DetailSelectorData = {
      issueTitle: this.issue.title,
      dialogTitle: 'Assigned User:',
      currentItem: this.issue.assignedTo,
      items: this.mProject !== undefined ? this.mProject.users : [],
    }
    const dialogConfig = new MatDialogConfig()
    dialogConfig.data = selectorData

    this.mSubscription = this.dialog
      .open( IssueDetailSelectorComponent, dialogConfig )
      .afterClosed()
      .subscribe( ( result: DetailSelectorResult ) => {
        if( result.accepted ) {
          const newUser = this.mProject?.users.find( c => c.id === result.selectedId )

          if( newUser !== undefined ) {
            this.issueService.UpdateAssignedUser( { ...this.issue, assignedTo: newUser } )
          }
        }
      } )
  }

  onWorkflowState() {
    const selectorData: DetailSelectorData = {
      issueTitle: this.issue.title,
      dialogTitle: 'Workflow State:',
      currentItem: this.issue.workflowState,
      items: this.mProject !== undefined ? this.mProject.workflowStates : [],
    }
    const dialogConfig = new MatDialogConfig()
    dialogConfig.data = selectorData

    this.mSubscription = this.dialog
      .open( IssueDetailSelectorComponent, dialogConfig )
      .afterClosed()
      .subscribe( ( result: DetailSelectorResult ) => {
        if( result.accepted ) {
          const newState = this.mProject?.workflowStates.find( c => c.id === result.selectedId )

          if( newState !== undefined ) {
            this.issueService.UpdateWorkflowState( { ...this.issue, workflowState: newState } )
          }
        }
      } )
  }

  ngOnDestroy() {
    this.mSubscription?.unsubscribe()
    this.mSubscription = undefined
  }
}
