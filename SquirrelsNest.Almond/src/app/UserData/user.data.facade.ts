import {Injectable} from '@angular/core'
import {Store} from '@ngrx/store'
import {take} from 'rxjs'
import {AppState} from '../Store/app.reducer'
import {getUserData} from '../Store/app.selectors'
import {UserData} from './user.data'
import {ClearUserData, UpdateUserData} from './user.data.actions'
import {UserDataService} from './user.data.service'
import {initialUserDataState} from './user.data.state'

@Injectable( {
  providedIn: 'root'
} )
export class UserDataFacade {
  constructor( private store: Store<AppState>, private userDataService: UserDataService ) {
  }

  RetrieveUserData(): void {
    this.userDataService.LoadUserData()
      .subscribe( payload => {
        if( payload?.userData?.data != null ) {
          if( payload.userData.data.length > 0 ) {
            const userData = JSON.parse( payload.userData.data ) as UserData

            if( userData != null ) {
              this.store.dispatch( new UpdateUserData( userData ) )
            }
          }
        }
      } )
  }

  GetCurrentUserData(): UserData {
    let retValue: UserData | null

    this.store.select( getUserData ).pipe( take( 1 ) ).subscribe( state => retValue = state )

    // @ts-ignore
    return retValue ?? initialUserDataState
  }

  ClearUserData(): void {
    this.store.dispatch( new ClearUserData() )
  }
}
