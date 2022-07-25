import {Injectable} from '@angular/core'
import {Router} from '@angular/router'
import {Store} from '@ngrx/store'
import {Apollo} from 'apollo-angular'

import {LoginInput, Mutation} from '../Data/graphQlTypes'
import {LoginMutation} from '../Data/mutationStatements'
import {AppState} from '../Store/app.reducer'
import {AuthFailed, AuthRequested, LoginCompleted, Logout} from './auth.actions'
import {clearAuthenticationToken} from './jwtSupport'

@Injectable()
export class AuthService {
  constructor( private apollo: Apollo, private store: Store<AppState>, private router: Router ) {
  }

  Login( loginName: string, password: string ) {
    const loginInput: LoginInput = { email: loginName, password: password }

    this.store.dispatch( new AuthRequested() )

    let subscription = this.apollo.mutate<Mutation, any>( {
      mutation: LoginMutation,
      variables: { loginInput: loginInput }
    } )
      .subscribe( result => {
        if( result.errors != null ) {
          this.store.dispatch( new AuthFailed() )
        }
        else if( result.data?.login != null ) {
          this.store.dispatch( new LoginCompleted( result.data.login ) )
        }

        this.router.navigate( ['issues'] ).then()

        subscription.unsubscribe()
      } )
  }

  Logout() {
    this.store.dispatch( new Logout() )

    clearAuthenticationToken()
    this.apollo.client.resetStore().then()
    this.router.navigate( ['login'] ).then()
  }
}
