import {Component, Input} from '@angular/core'
import {ClUser} from '../../Data/graphQlTypes'

@Component( {
  selector: 'sn-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.css']
} )
export class UserDetailComponent {
  @Input() user!: ClUser
  isHovering: boolean = false

  constructor() {
  }

  role() : string {
    let retValue = ''

    if( this.user !== null ) {
      if( this.user.claims.find( c => c.type === 'role' && c.value === 'user')) {
        retValue = 'user'
      }
      if( this.user.claims.find( c => c.type === 'role' && c.value === 'admin')) {
        retValue = 'administrator'
      }
    }

    return retValue
  }

  onEditUser() {
  }

  onDeleteUser() {
  }
}
