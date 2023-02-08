import {Component} from '@angular/core'
import {MatDialog, MatDialogConfig} from '@angular/material/dialog'
import {AddUserInput} from '../../Data/graphQlTypes'
import {UserCreateDialogComponent, UserCreateResult} from '../user-create-dialog/user-create-dialog.component'
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
    const dialogConfig = new MatDialogConfig()

    this.dialog
      .open( UserCreateDialogComponent, dialogConfig )
      .afterClosed()
      .subscribe( ( result: UserCreateResult ) => {
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
