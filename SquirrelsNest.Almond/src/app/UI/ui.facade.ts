import {Injectable} from '@angular/core'
import {MatDialog, MatDialogConfig} from '@angular/material/dialog'
import {Store} from '@ngrx/store'
import {Observable, take} from 'rxjs'
import {AppState} from '../Store/app.reducer'
import {
  getDisplayCompletedIssues,
  getDisplayOnlyMyIssues,
  getIssueDisplayStyle,
  getServiceActivity
} from '../Store/app.selectors'
import {MessageDialogComponent, MessageInput} from './message-dialog/message-dialog.component'
import {DisplayCompletedIssues, DisplayOnlyMyIssues, SetIssueListStyle} from './ui.actions'
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

  GetDisplayCompletedIssues() {
    let retValue = false

    this.store.select( getDisplayCompletedIssues ).pipe( take( 1 ) ).subscribe( state => retValue = state )

    return retValue
  }

  SetDisplayCompletedIssues( state: boolean ) {
    this.store.dispatch( new DisplayCompletedIssues( state ) )
  }

  GetDisplayOnlyMyIssues$() {
    return this.store.select( getDisplayOnlyMyIssues )
  }

  GetDisplayOnlyMyIssues() {
    let retValue = false

    this.store.select( getDisplayOnlyMyIssues ).pipe( take( 1 ) ).subscribe( state => retValue = state )

    return retValue
  }

  SetDisplayOnlyMyIssues( state: boolean ) {
    this.store.dispatch( new DisplayOnlyMyIssues( state ) )
  }

  ToggleIssueListStyle() {
    this.store.dispatch( new ToggleIssueListStyle() )
  }

  SetIssueListStyle( style: eIssueDisplayStyle ) {
    this.store.dispatch( new SetIssueListStyle( style ) )
  }

  GetIssueListDisplayStyle$(): Observable<eIssueDisplayStyle> {
    return this.store.select( getIssueDisplayStyle )
  }

  GetIssueListDisplayStyle() {
    let retValue = eIssueDisplayStyle.FULL_DETAILS

    this.store.select( getIssueDisplayStyle ).pipe( take( 1 ) ).subscribe( state => retValue = state )

    return retValue
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

  GetServiceActivity(): string {
    let retValue = ''

    this.store.select( getServiceActivity ).pipe( take( 1 ) ).subscribe( value => retValue = value )

    return retValue
  }
}
