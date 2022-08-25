import {Component} from '@angular/core'
import {MatDialog, MatDialogConfig} from '@angular/material/dialog'
import {AddUserInput} from '../../Data/graphQlTypes'
import {UserEditData, UserEditDialogComponent, UserEditResult} from '../user-edit-dialog/user-edit-dialog.component'
import {UsersFacade} from '../user.facade'

@Component( {
  selector: 'sn-user-header',
  templateUrl: './user-header.component.html',
  styleUrls: ['./user-header.component.css']
} )
export class UserHeaderComponent {

  constructor( private dialog: MatDialog, private userFacade: UsersFacade ) {
  }

  onAddUser() {
    const newUser: UserEditData = {
      name: '',
      email: '',
      password: '',
    }

    const dialogConfig = new MatDialogConfig()
    dialogConfig.data = newUser

    this.dialog
      .open( UserEditDialogComponent, dialogConfig )
      .afterClosed()
      .subscribe( ( result: UserEditResult ) => {
        if( result.accepted ) {
          const newUser: AddUserInput = {
            name: result.name,
            loginName: result.email,
            email: result.email,
            password: result.password
          }
          this.userFacade.AddUser( newUser )
        }
      } )

  }
}
