import {Component, OnDestroy, OnInit} from '@angular/core'
import {MatDialog, MatDialogConfig} from '@angular/material/dialog'
import {Store} from '@ngrx/store'
import {Observable, Subscription, take} from 'rxjs'
import {ClIssue, ClProject} from '../../Data/graphQlTypes'
import {AppState} from '../../Store/app.reducer'
import {getSelectedProject} from '../../Store/app.selectors'
import {ToggleIssueListStyle} from '../../UI/ui.actions'
import {
  IssueEditData,
  IssueEditDialogComponent,
  IssueEditResult
} from '../issue-edit-dialog/issue-edit-dialog.component'
import {IssueService} from '../issues.service'

@Component( {
  selector: 'sn-issue-header',
  templateUrl: './issue-header.component.html',
  styleUrls: ['./issue-header.component.css']
} )
export class IssueHeaderComponent implements OnInit, OnDestroy {
  mDialogSubscription: Subscription | undefined

  currentProject$: Observable<ClProject | null>

  constructor( private store: Store<AppState>, private dialog: MatDialog, private issueService: IssueService ) {
    this.currentProject$ = new Observable<ClProject>()
  }

  ngOnInit(): void {
    this.currentProject$ = this.store.select( getSelectedProject )
  }

  onToggleListStyle() {
    this.store.dispatch( new ToggleIssueListStyle() )
  }

  onCreateNewIssue() {
    const currentProject = this.getCurrentProject()

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
            this.issueService.AddIssue( result.issue )
          }
        } )
    }
  }

  private getCurrentProject(): ClProject | null {
    let currentProject: ClProject | null = null

    this.store.select( getSelectedProject ).pipe( take( 1 ) ).subscribe( project => currentProject = project )

    return currentProject
  }

  ngOnDestroy() {
    this.mDialogSubscription?.unsubscribe()
    this.mDialogSubscription = undefined
  }
}
