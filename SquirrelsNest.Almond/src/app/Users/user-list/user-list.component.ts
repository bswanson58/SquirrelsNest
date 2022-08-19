import {Component, OnInit} from '@angular/core'
import {Observable} from 'rxjs'
import {ClUser} from '../../Data/graphQlTypes'
import {UsersFacade} from '../user.facade'

@Component( {
  selector: 'sn-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
} )
export class UserListComponent implements OnInit {
  users$: Observable<ClUser[]>

  constructor( private usersFacade: UsersFacade ) {
    this.users$ = usersFacade.GetCurrentUsersList$()
  }

  ngOnInit(): void {
    this.usersFacade.LoadUsers()
  }
}
