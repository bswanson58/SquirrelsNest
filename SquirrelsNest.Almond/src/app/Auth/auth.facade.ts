import {Injectable} from '@angular/core'
import {Store} from '@ngrx/store'
import {Observable} from 'rxjs'
import {IssuesFacade} from '../Issues/issues.facade'
import {ProjectFacade} from '../Projects/project.facade'
import {AppState} from '../Store/app.reducer'
import {getAuthenticationClaims} from '../Store/app.selectors'
import {UserDataFacade} from '../UserData/user.data.facade'
import {claim} from './auth.models'
import {AuthService} from './auth.service'

@Injectable( {
  providedIn: 'root'
} )
export class AuthFacade {
  constructor( private store: Store<AppState>,
               private authService: AuthService,
               private projectFacade: ProjectFacade,
               private issuesFacade: IssuesFacade,
               private userDataFacade: UserDataFacade ) {
  }

  Login( loginName: string, password: string ) {
    this.authService.Login( loginName, password )
  }

  Logout() {
    this.authService.Logout()
    this.projectFacade.ClearState()
    this.issuesFacade.ClearIssues()
    this.userDataFacade.ClearUserData()
  }

  GetAuthenticationClaims$(): Observable<claim[]> {
    return this.store.select( getAuthenticationClaims )
  }
}
