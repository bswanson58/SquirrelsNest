import {Injectable, OnDestroy} from '@angular/core'
import {Store} from '@ngrx/store'
import {filter, map, Subscription, take, tap, combineLatest} from 'rxjs'
import {ProjectFacade} from '../Projects/project.facade'
import {SelectProject} from '../Projects/projects.actions'
import {AppState} from '../Store/app.reducer'
import {getAuthenticationClaims} from '../Store/app.selectors'
import {UserData} from './user.data'
import {UserDataFacade} from './user.data.facade'
import {UserDataService} from './user.data.service'
import {getUserDataState, UserDataState} from './user.data.state'

@Injectable( {
  providedIn: 'root'
} )
export class UserDataManager implements OnDestroy {
  private subscriptions: Subscription
  private monitoredData: Subscription

  constructor( private store: Store<AppState>, private userDataService: UserDataService, private userDataFacade: UserDataFacade, private projectFacade: ProjectFacade ) {
    this.subscriptions = new Subscription()
    this.monitoredData = new Subscription()

    this.subscriptions.add(
      store.select( getAuthenticationClaims )
        .pipe(
          map( claims => claims.find( c => c.name === 'email' ) ),
          map( claim => claim != null ? claim.value : '' ),
          tap( email => {
            if( email.length > 0 ) {
              this.onUserLogin()
            }
            else {
              this.unsubscribeFromMonitoredData()
            }
          } ),
        )
        .subscribe()
    )
  }

  private onUserLogin() {
    combineLatest( [this.projectFacade.GetProjectList$(), this.store.select( getUserDataState )] )
      .pipe(
        filter( ( [projects, _] ) => projects.length > 0 ),
        filter( ( [_, userData] ) => userData.lastProject.length > 0 ),
        tap( ( [_, userData] ) => {
          this.applyUserData( userData )
        } ),
        take( 1 ),
      )
      .subscribe()

    this.userDataFacade.RetrieveUserData()
    this.projectFacade.LoadProjects()
  }

  private subscribeToMonitoredData() {
    this.unsubscribeFromMonitoredData()

    this.monitoredData.add(
      this.projectFacade.GetCurrentProject$()
        .pipe(
          filter( project => project != null ),
          tap( _ => {
              console.assert( _ != null )
              const userData = this.createUserData()

              if( this.shouldSaveUserData( userData ) ) {
                this.userDataService.SaveUserData( userData )
              }
            }
          ),
        )
        .subscribe()
    )
  }

  private unsubscribeFromMonitoredData() {
    this.monitoredData?.unsubscribe()
    this.monitoredData = new Subscription()
  }

  private createUserData(): UserData {
    const currentProject = this.projectFacade.GetCurrentProject()

    return {
      currentProject: currentProject ? currentProject.id : ''
    }
  }

  private applyUserData( userData: UserDataState ) {
    this.unsubscribeFromMonitoredData()

    if( userData.lastProject.length > 0 ) {
      const project = this.projectFacade.FindProject( userData.lastProject )

      if( project != null ) {
        this.store.dispatch( new SelectProject( project ) )
      }
    }

    this.subscribeToMonitoredData()
  }

  private shouldSaveUserData( userData: UserData ): boolean {
    const currentUserData = this.userDataFacade.GetCurrentUserData()

    return userData.currentProject !== currentUserData.currentProject
  }

  ngOnDestroy() {
    this.subscriptions?.unsubscribe()
    this.monitoredData?.unsubscribe()
  }
}
