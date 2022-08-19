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

  onEditUser() {
  }

  onDeleteUser() {
  }
}
