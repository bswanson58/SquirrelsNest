import {Injectable} from '@angular/core'
import {Store} from '@ngrx/store'
import {AppState} from '../Store/app.reducer'
import {UserData} from './user.data'
import {UpdateUserData} from './user.data.actions'
import {UserDataService} from './user.data.service'

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
}
