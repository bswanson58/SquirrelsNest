import {Component} from '@angular/core'
import {MatDialog, MatDialogConfig} from '@angular/material/dialog'
import {UserEditData, UserEditDialogComponent, UserEditResult} from '../user-edit-dialog/user-edit-dialog.component'

@Component( {
  selector: 'sn-user-header',
  templateUrl: './user-header.component.html',
  styleUrls: ['./user-header.component.css']
} )
export class UserHeaderComponent {

  constructor( private dialog: MatDialog ) {
  }

  onAddUser() {
    const newUser: UserEditData = {
      name: '',
      email: ''
    }

    const dialogConfig = new MatDialogConfig()
    dialogConfig.data = newUser

    this.dialog
      .open( UserEditDialogComponent, dialogConfig )
      .afterClosed()
      .subscribe( ( result: UserEditResult ) => {
        if( result.accepted ) {

        }
      } )

  }
}
