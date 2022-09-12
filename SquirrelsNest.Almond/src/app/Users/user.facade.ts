import {Injectable} from '@angular/core'
import {MatDialog} from '@angular/material/dialog'
import {Store} from '@ngrx/store'
import {Observable} from 'rxjs'
import {AddUserInput, ClUser} from '../Data/graphQlTypes'
import {AppState} from '../Store/app.reducer'
import {
  getLoadedUsers,
  getServerHasMoreUsers,
  getTotalUsers,
  getUsers
} from '../Store/app.selectors'
import {ClearUsers} from './user.actions'
import {UserMutationsService} from './user.mutations.service'
import {UserService} from './user.service'

@Injectable( {
  providedIn: 'root'
} )
export class UsersFacade {
  constructor( private store: Store<AppState>,
               private userService: UserService,
               private userMutationService: UserMutationsService,
               private dialog: MatDialog ) {
  }

  AddUser( user: AddUserInput ) {
    this.userMutationService.AddUser( user )
  }

  ClearUsers() {
    this.store.dispatch( new ClearUsers() )
  }

  DeleteUser( user: ClUser ) {
    this.userMutationService.DeleteUser( user )
  }

  LoadUsers() {
    this.userService.LoadUsers()
  }

  LoadMoreUsers() {
    this.userService.LoadMoreUsers()
  }

  UpdateUserRoles( user: ClUser ) {
    this.userMutationService.UpdateUserRoles( user )
  }

  UpdateUserPassword( user: ClUser, currentPassword: string, newPassword: string ) {
    this.userMutationService.UpdateUserPassword( user, currentPassword, newPassword )
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