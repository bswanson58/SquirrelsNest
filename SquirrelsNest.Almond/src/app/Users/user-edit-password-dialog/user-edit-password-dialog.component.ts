import {Component, Inject} from '@angular/core'
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog'
import {ClUser} from '../../Data/graphQlTypes'

export interface EditUserPasswordInput {
  user: ClUser
}

export interface EditUserPasswordResult {
  accepted: boolean,
  user: ClUser,
  currentPassword: string,
  newPassword: string,
}

@Component( {
  selector: 'sn-user-edit-password-dialog',
  templateUrl: './user-edit-password-dialog.component.html',
  styleUrls: ['./user-edit-password-dialog.component.css']
} )
export class UserEditPasswordDialogComponent {
  currentPassword: string
  newPassword: string
  retypePassword: string

  constructor( private dialogRef: MatDialogRef<UserEditPasswordDialogComponent>,
               @Inject( MAT_DIALOG_DATA ) private dialogData: EditUserPasswordInput ) {
    this.currentPassword = ''
    this.newPassword = ''
    this.retypePassword = ''

    this.dialogRef.updateSize( '375px' )
  }

  onClose() {
    const result: EditUserPasswordResult = {
      accepted: true,
      user: this.dialogData.user,
      currentPassword: this.currentPassword,
      newPassword: this.newPassword
    }

    this.dialogRef.close( result )
  }

  onCancel() {
    this.dialogRef.close( { accepted: false } as EditUserPasswordResult )
  }
}
