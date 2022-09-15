import {Injectable, OnDestroy} from '@angular/core'
import {MatSnackBar} from '@angular/material/snack-bar'
import {Store} from '@ngrx/store'
import {Subscription} from 'rxjs'
import {AppState} from '../Store/app.reducer'
import {getLastError} from '../Store/app.selectors'
import {ErrorPanelComponent} from './error-panel/error-panel.component'

@Injectable( {
  providedIn: 'root'
} )
export class MessageReporter implements OnDestroy {
  private lastErrorSubscription: Subscription | undefined

  constructor( private store: Store<AppState>, private messageProvider: MatSnackBar ) {
    this.lastErrorSubscription =
      this.store.select( getLastError )
        .subscribe(
          message => {
            if( message.length > 0 ) {
              this.messageProvider.openFromComponent( ErrorPanelComponent, {
                data: message,
                duration: 3000,
                panelClass: 'snackbar-error'
              } )
            }
          }
        )
  }

  ngOnDestroy() {
    this.lastErrorSubscription?.unsubscribe()
  }
}
