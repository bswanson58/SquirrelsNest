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

  constructor( private store: Store<AppState>, private dialog: MatDialog ) {
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
        console.log( result )
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
        console.log( result )
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
        console.log( result )
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
        console.log( result )
      } )
  }

  ngOnDestroy() {
    this.mSubscription?.unsubscribe()
    this.mSubscription = undefined
  }
}
