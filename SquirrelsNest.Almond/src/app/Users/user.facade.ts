import {Injectable} from '@angular/core'
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
               private userMutationService: UserMutationsService ) {
  }

  AddUser( user: AddUserInput ) {
    this.userMutationService.AddUser( user, () => {
    } )
  }

  AddUserWithCallback( user: AddUserInput, callback: ( success: boolean, message: string ) => void ) {
    this.userMutationService.AddUser( user, callback )
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
