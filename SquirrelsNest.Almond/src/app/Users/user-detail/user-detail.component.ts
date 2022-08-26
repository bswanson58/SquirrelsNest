import {Component, Input} from '@angular/core'
import {MatDialog, MatDialogConfig} from '@angular/material/dialog'
import {ClUser} from '../../Data/graphQlTypes'
import {
  ConfirmDialogComponent,
  ConfirmDialogData,
  ConfirmDialogResult
} from '../../UI/confirm-dialog/confirm-dialog.component'
import {UsersFacade} from '../user.facade'

@Component( {
  selector: 'sn-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.css']
} )
export class UserDetailComponent {
  @Input() user!: ClUser
  isHovering: boolean = false

  constructor( private usersFacade: UsersFacade, private dialog: MatDialog ) {
  }

  role(): string {
    let retValue = ''

    if( this.user !== null ) {
      if( this.user.claims.find( c => c.type === 'role' && c.value === 'user' ) ) {
        retValue = 'user'
      }
      if( this.user.claims.find( c => c.type === 'role' && c.value === 'admin' ) ) {
        retValue = 'administrator'
      }
    }

    return retValue
  }

  onEditUser() {
  }

  onDeleteUser() {
    const dialogData: ConfirmDialogData = {
      prompt: 'Do you want to delete this user?',
      promptDetail: this.user.name
    }
    const dialogConfig = new MatDialogConfig()
    dialogConfig.data = dialogData

    this.dialog
      .open( ConfirmDialogComponent, dialogConfig )
      .afterClosed()
      .subscribe( ( result: ConfirmDialogResult ) => {
        if( result.accepted ) {
          this.usersFacade.DeleteUser( this.user )
        }
      } )
  }
}
