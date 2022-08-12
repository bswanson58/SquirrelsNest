import {Injectable} from '@angular/core'
import {Store} from '@ngrx/store'
import {Observable} from 'rxjs'
import {AppState} from '../Store/app.reducer'
import {getAuthenticationClaims} from '../Store/app.selectors'
import {claim} from './auth.models'
import {AuthService} from './auth.service'

@Injectable( {
  providedIn: 'root'
} )
export class AuthFacade {
  constructor( private store: Store<AppState>, private authService: AuthService ) {
  }

  Login( loginName: string, password: string ) {
    this.authService.Login( loginName, password )
  }

  Logout() {
    this.authService.Logout()
  }

  GetAuthenticationClaims(): Observable<claim[]> {
    return = this.store.select(getAuthenticationClaims)
  }
}
