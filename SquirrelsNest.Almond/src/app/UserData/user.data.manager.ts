import {Injectable, OnDestroy} from '@angular/core'
import {Store} from '@ngrx/store'
import {combineLatest, filter, firstValueFrom, map, Subscription, take, tap} from 'rxjs'
import {ProjectFacade} from '../Projects/project.facade'
import {SelectProject} from '../Projects/projects.actions'
import {AppState} from '../Store/app.reducer'
import {getAuthenticationClaims} from '../Store/app.selectors'
import {UiFacade} from '../UI/ui.facade'
import {UserData} from './user.data'
import {UserDataFacade} from './user.data.facade'
import {UserDataService} from './user.data.service'

@Injectable( {
  providedIn: 'root'
} )
export class UserDataManager implements OnDestroy {
  private subscriptions: Subscription
  private monitoredData: Subscription

  constructor( private store: Store<AppState>,
               private userDataService: UserDataService,
               private userDataFacade: UserDataFacade,
               private projectFacade: ProjectFacade,
               private uiFacade: UiFacade ) {
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

  private onUserLogin(): void {
    combineLatest( [this.projectFacade.GetProjectList$(), this.userDataFacade.GetCurrentUserData$()] )
      .pipe(
        filter( ( [projects, _] ) => projects.length > 0 ),
        tap( ( [_, userData] ) => {
          this.applyUserData( userData )
        } ),
        take( 1 ),
      )
      .subscribe()

    this.userDataFacade.RetrieveUserData()
    this.projectFacade.LoadProjects()
  }

  private subscribeToMonitoredData(): void {
    this.unsubscribeFromMonitoredData()

    this.monitoredData.add(
      this.projectFacade.GetCurrentProject$()
        .pipe(
          filter( project => project != null ),
          tap( _ => this.updateUserData() ),
        )
        .subscribe()
    )

    this.monitoredData.add(
      this.uiFacade.GetIssueListDisplayStyle$()
        .subscribe( () => this.updateUserData() )
    )

    this.monitoredData.add(
      this.uiFacade.GetDisplayOnlyMyIssues$()
        .subscribe( () => this.updateUserData() )
    )

    this.monitoredData.add(
      this.uiFacade.GetDisplayCompletedIssues$()
        .subscribe( () => this.updateUserData() )
    )
  }

  private async updateUserData(): Promise<any> {
    const userData = await this.createUserData()

    if( await this.shouldSaveUserData( userData ) ) {
      this.userDataService.SaveUserData( userData )
    }
  }

  private unsubscribeFromMonitoredData(): void {
    this.monitoredData?.unsubscribe()
    this.monitoredData = new Subscription()
  }

  private async createUserData(): Promise<UserData> {
    const currentProject = await firstValueFrom( this.projectFacade.GetCurrentProject$() )

    return {
      currentProject: currentProject ? currentProject.id : '',
      displayStyle: await this.uiFacade.GetIssueListDisplayStyle(),
      displayCompletedIssues: await this.uiFacade.GetDisplayCompletedIssues(),
      displayOnlyMyIssues: await this.uiFacade.GetDisplayOnlyMyIssues()
    }
  }

  private applyUserData( userData: UserData ): void {
    if( userData.currentProject.length > 0 ) {
      this.unsubscribeFromMonitoredData()

      const project = this.projectFacade.FindProject( userData.currentProject )

      if( project != null ) {
        this.store.dispatch( new SelectProject( project ) )
      }

      this.uiFacade.SetDisplayCompletedIssues( userData.displayCompletedIssues )
      this.uiFacade.SetDisplayOnlyMyIssues( userData.displayOnlyMyIssues )
      this.uiFacade.SetIssueListStyle( userData.displayStyle )

      this.subscribeToMonitoredData()
    }
  }

  private async shouldSaveUserData( userData: UserData ): Promise<boolean> {
    const currentUserData = await this.userDataFacade.GetCurrentUserData()

    return (
      userData.currentProject.length > 0 &&
      (userData.currentProject !== currentUserData.currentProject ||
        userData.displayOnlyMyIssues !== currentUserData.displayOnlyMyIssues ||
        userData.displayCompletedIssues !== currentUserData.displayCompletedIssues ||
        userData.displayStyle !== currentUserData.displayStyle)
    )
  }

  ngOnDestroy() {
    this.subscriptions?.unsubscribe()
    this.monitoredData?.unsubscribe()
  }
}
