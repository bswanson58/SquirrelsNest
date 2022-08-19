import {Injectable} from '@angular/core'
import {MatDialog} from '@angular/material/dialog'
import {Store} from '@ngrx/store'
import {Observable} from 'rxjs'
import {ClIssue, ClUser} from '../Data/graphQlTypes'
import {AppState} from '../Store/app.reducer'
import {
  getIssues,
  getLoadedIssues, getLoadedUsers,
  getServerHasMoreIssues,
  getServerHasMoreUsers,
  getTotalIssues, getTotalUsers,
  getUsers
} from '../Store/app.selectors'
import {ClearUsers} from './user.actions'
import {UserService} from './user.service'

@Injectable( {
  providedIn: 'root'
} )
export class UsersFacade {
  constructor( private store: Store<AppState>,
               private userService: UserService,
               private dialog: MatDialog ) {
  }

  ClearUsers() {
    this.store.dispatch( new ClearUsers() )
  }

  LoadUsers() {
    this.userService.LoadUsers()
  }

  LoadMoreUsers() {
    this.userService.LoadMoreUsers()
  }

  GetCurrentUsersList$(): Observable<ClUser[]> {
    return this.store.select( getUsers )
  }

  GetServerHasMoreUsers$(): Observable<boolean> {
    return this.store.select( getServerHasMoreUsers )
  }

  GetTotalUsers$(): Observable<number> {
    return this.store.select( getTotalUsers )
  }

  GetLoadedUsers$(): Observable<number> {
    return this.store.select( getLoadedUsers )
  }
}
