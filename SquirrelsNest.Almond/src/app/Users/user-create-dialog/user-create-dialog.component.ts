import {Component} from '@angular/core'
import {MatDialogRef} from '@angular/material/dialog'

export interface UserCreateResult {
  accepted: boolean,
  name: string,
  email: string,
  password: string,
}

@Component( {
  selector: 'sn-user-create-dialog',
  templateUrl: './user-create-dialog.component.html',
  styleUrls: ['./user-create-dialog.component.css']
} )
export class UserCreateDialogComponent {
  name: string
  email: string
  password: string
  retypedPassword: string

  constructor( private dialogRef: MatDialogRef<UserCreateDialogComponent> ) {
    this.name = ''
    this.email = ''
    this.password = ''
    this.retypedPassword = ''

    this.dialogRef.updateSize( '500px' )
  }

  onClose() {
    const result: UserCreateResult = {
      accepted: true,
      name: this.name,
      email: this.email,
      password: this.password,
    }

    this.dialogRef.close( result )
  }

  onCancel() {
    this.dialogRef.close( { accepted: false } as UserCreateResult )
  }
}
