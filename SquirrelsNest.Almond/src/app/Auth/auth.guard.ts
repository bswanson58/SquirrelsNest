import {Injectable} from '@angular/core'
import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot} from '@angular/router'
import {Store} from '@ngrx/store'
import {Observable, take, tap} from 'rxjs'
import {AppState} from '../Store/app.reducer'
import {getIsAuthenticated} from '../Store/app.selectors'

@Injectable()
export class AuthGuard implements CanActivate {

  constructor( private store: Store<AppState>, private router: Router ) {
  }

  canActivate( route: ActivatedRouteSnapshot, state: RouterStateSnapshot ): Observable<boolean> | boolean {
    return this.store.select( getIsAuthenticated )
      .pipe(
        take( 1 ),
        tap( isAuth => {
            if( !isAuth ) {
              this.router.navigate( ['login'] ).then()
            }
          }
        )
      )
  }
}
