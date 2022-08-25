import {Component, Inject} from '@angular/core'
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog'

export interface UserEditData {
  name: string,
  email: string
}

export interface UserEditResult {
  accepted: boolean,
  name: string,
  email: string
}

@Component({
  selector: 'sn-user-edit-dialog',
  templateUrl: './user-edit-dialog.component.html',
  styleUrls: ['./user-edit-dialog.component.css']
})
export class UserEditDialogComponent {
  name: string
  email: string

  constructor( private dialogRef: MatDialogRef<UserEditDialogComponent>,
               @Inject( MAT_DIALOG_DATA ) private dialogData: UserEditData ) {
    this.name = dialogData.name
    this.email = dialogData.email

    this.dialogRef.updateSize( '500px' )
  }

  onClose() {
    const result: UserEditResult = {
      accepted: true,
      name: this.name,
      email: this.email,
    }

    this.dialogRef.close( result )
  }

  onCancel() {
    this.dialogRef.close( { accepted: false } as UserEditResult )
  }
}
