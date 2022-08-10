import {Component, OnDestroy, OnInit} from '@angular/core'
import {MatDialog, MatDialogConfig} from '@angular/material/dialog'
import {Observable, Subscription} from 'rxjs'
import {ClIssue, ClProject} from '../../Data/graphQlTypes'
import {ProjectFacade} from '../../Projects/project.facade'
import {
  IssueEditData,
  IssueEditDialogComponent,
  IssueEditResult
} from '../issue-edit-dialog/issue-edit-dialog.component'
import {IssuesFacade} from '../issues.facade'

@Component( {
  selector: 'sn-issue-header',
  templateUrl: './issue-header.component.html',
  styleUrls: ['./issue-header.component.css']
} )
export class IssueHeaderComponent implements OnInit, OnDestroy {
  mDialogSubscription: Subscription | undefined

  currentProject$: Observable<ClProject | null>

  constructor( private dialog: MatDialog, private projectFacade: ProjectFacade, private issuesFacade: IssuesFacade ) {
    this.currentProject$ = new Observable<ClProject>()
  }

  ngOnInit(): void {
    this.currentProject$ = this.projectFacade.GetCurrentProject$()
  }

  onToggleListStyle() {
    this.issuesFacade.ToggleIssueListStyle()
  }

  onCreateNewIssue() {
    const currentProject = this.projectFacade.GetCurrentProject()

    if( currentProject !== null ) {
      const dialogData: IssueEditData = {
        issue: {} as ClIssue,
        project: currentProject
      }
      const dialogConfig = new MatDialogConfig()
      dialogConfig.data = dialogData

      this.mDialogSubscription = this.dialog
        .open( IssueEditDialogComponent, dialogConfig )
        .afterClosed()
        .subscribe( ( result: IssueEditResult ) => {
          if( (result.accepted) &&
            (result.issue !== null) ) {
            this.issuesFacade.AddIssue( result.issue )
          }
        } )
    }
  }

  ngOnDestroy() {
    this.mDialogSubscription?.unsubscribe()
    this.mDialogSubscription = undefined
  }
}
