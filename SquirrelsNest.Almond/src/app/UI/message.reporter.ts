import {Injectable, OnDestroy} from '@angular/core'
import {MatSnackBar, MatSnackBarRef} from '@angular/material/snack-bar'
import {Store} from '@ngrx/store'
import {Subscription} from 'rxjs'
import {AppState} from '../Store/app.reducer'
import {getLastError, getServiceIsActive} from '../Store/app.selectors'
import {ErrorPanelComponent} from './error-panel/error-panel.component'
import {ServiceActivityPanelComponent} from './service-activity-panel/service-activity-panel.component'
import {ClearError} from './ui.actions'
import {UiFacade} from './ui.facade'

@Injectable( {
  providedIn: 'root'
} )
export class MessageReporter implements OnDestroy {
  private lastErrorSubscription: Subscription | undefined
  private serviceActivitySubscription: Subscription | undefined
  private serviceActivitySnackBar: MatSnackBarRef<ServiceActivityPanelComponent> | null

  constructor( private store: Store<AppState>, private messageProvider: MatSnackBar, private uiFacade: UiFacade ) {
    this.serviceActivitySnackBar = null

    this.lastErrorSubscription =
      this.store.select( getLastError )
        .subscribe(
          message => {
            if( message.length > 0 ) {
              this.messageProvider.openFromComponent( ErrorPanelComponent, {
                data: message,
                duration: 5000,
                panelClass: ['snackbar-error']
              } )

              this.store.dispatch( new ClearError() )
            }
          }
        )

    this.serviceActivitySubscription =
      this.store.select( getServiceIsActive )
        .subscribe( state => {
          if( state ) {
            this.serviceActivitySnackBar =
              this.messageProvider.openFromComponent( ServiceActivityPanelComponent, {
                data: this.uiFacade.GetServiceActivity(),
                panelClass: ['snackbar-service',]
              } )
          }
          else {
            this.serviceActivitySnackBar?.dismiss()
            this.serviceActivitySnackBar = null
          }
        } )
  }

  ngOnDestroy() {
    this.lastErrorSubscription?.unsubscribe()
    this.serviceActivitySubscription?.unsubscribe()
  }
}
