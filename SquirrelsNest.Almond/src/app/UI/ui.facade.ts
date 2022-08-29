import {Injectable} from '@angular/core'
import {MatDialog, MatDialogConfig} from '@angular/material/dialog'
import {Store} from '@ngrx/store'
import {Observable} from 'rxjs'
import {AppState} from '../Store/app.reducer'
import {getDisplayCompletedIssues, getDisplayOnlyMyIssues, getIssueDisplayStyle} from '../Store/app.selectors'
import {MessageDialogComponent, MessageInput} from './message-dialog/message-dialog.component'
import {DisplayCompletedIssues, DisplayOnlyMyIssues} from './ui.actions'
import {eIssueDisplayStyle} from './ui.state'
import {ToggleIssueListStyle} from './ui.actions'

@Injectable( {
  providedIn: 'root'
} )
export class UiFacade {
  constructor( private store: Store<AppState>, private dialog: MatDialog ) {
  }

  GetDisplayCompletedIssues$() {
    return this.store.select( getDisplayCompletedIssues )
  }

  SetDisplayCompletedIssues( state: boolean ) {
    this.store.dispatch( new DisplayCompletedIssues( state ) )
  }

  GetDisplayOnlyMyIssues$() {
    return this.store.select( getDisplayOnlyMyIssues )
  }

  SetDisplayOnlyMyIssues( state: boolean ) {
    this.store.dispatch( new DisplayOnlyMyIssues( state ) )
  }

  ToggleIssueListStyle() {
    this.store.dispatch( new ToggleIssueListStyle() )
  }

  GetIssueListDisplayStyle(): Observable<eIssueDisplayStyle> {
    return this.store.select( getIssueDisplayStyle )
  }

  DisplayMessage( title: string, message: string ) {
    const messageInput: MessageInput = {
      title: title,
      message: message
    }

    const dialogConfig = new MatDialogConfig()
    dialogConfig.data = messageInput

    this.dialog
      .open( MessageDialogComponent, dialogConfig )
      .afterClosed()
      .subscribe()
  }
}
