import {Component, Inject} from '@angular/core'
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog'
import {ClClaim, ClUser} from '../../Data/graphQlTypes'

export interface UserEditRolesData {
  user: ClUser
}

export interface UserEditRolesResult {
  accepted: boolean,
  user: ClUser,
  roles: ClClaim[]
}

@Component( {
  selector: 'sn-user-edit-roles-dialog',
  templateUrl: './user-edit-roles-dialog.component.html',
  styleUrls: ['./user-edit-roles-dialog.component.css']
} )
export class UserEditRolesDialogComponent {
  user: ClUser
  isDisabled: boolean
  isUser: boolean
  isAdmin: boolean

  constructor( private dialogRef: MatDialogRef<UserEditRolesDialogComponent>,
               @Inject( MAT_DIALOG_DATA ) private dialogData: UserEditRolesData ) {
    this.user = dialogData.user
    this.isDisabled = dialogData.user.claims.length === 0
    this.isUser = dialogData.user.claims.find( r => r.type === 'role' && r.value === 'user' ) !== undefined
    this.isAdmin = dialogData.user.claims.find( r => r.type === 'role' && r.value === 'admin' ) !== undefined

    this.dialogRef.updateSize( '350px' )
  }

  onChangeDisable() {
    if( this.isDisabled ) {
      this.isUser = false
      this.isAdmin = false
    }
  }

  onChangeRole() {
    this.isDisabled = !this.isUser && !this.isAdmin
  }

  onClose() {
    const result: UserEditRolesResult = {
      accepted: true,
      user: this.user,
      roles: []
    }

    if( !this.isDisabled ) {
      if( this.isUser ) {
        result.roles = [...result.roles, { type: 'role', value: 'user' }]
      }
      if( this.isAdmin ) {
        result.roles = [...result.roles, { type: 'role', value: 'admin' }]
      }
    }

    this.dialogRef.close( result )
  }

  onCancel() {
    this.dialogRef.close( { accepted: false } as UserEditRolesResult )
  }
}
