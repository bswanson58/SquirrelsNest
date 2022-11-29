import {Component, Inject} from '@angular/core'
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog'

export interface UserEditData {
  name: string,
  email: string,
  password: string
}

export interface UserEditResult {
  accepted: boolean,
  name: string,
  email: string,
  password: string,
}

@Component( {
  selector: 'sn-user-edit-dialog',
  templateUrl: './user-edit-dialog.component.html',
  styleUrls: ['./user-edit-dialog.component.css']
} )
export class UserEditDialogComponent {
  name: string
  email: string
  password: string

  constructor( private dialogRef: MatDialogRef<UserEditDialogComponent>,
               @Inject( MAT_DIALOG_DATA ) dialogData: UserEditData ) {
    this.name = dialogData.name
    this.email = dialogData.email
    this.password = dialogData.password

    this.dialogRef.updateSize( '500px' )
  }

  onClose() {
    const result: UserEditResult = {
      accepted: true,
      name: this.name,
      email: this.email,
      password: this.password,
    }

    this.dialogRef.close( result )
  }

  onCancel() {
    this.dialogRef.close( { accepted: false } as UserEditResult )
  }
}
