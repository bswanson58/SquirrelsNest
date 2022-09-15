import {Component, OnDestroy, OnInit} from '@angular/core'
import {Title} from '@angular/platform-browser'
import {NavigationStart, Router, Event as NavigationEvent} from '@angular/router'
import {Store} from '@ngrx/store'
import {Subscription, tap} from 'rxjs'
import {ProjectFacade} from './Projects/project.facade'
import {AppState} from './Store/app.reducer'
import {MessageReporter} from './UI/message.reporter'

@Component( {
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
} )
export class AppComponent implements OnInit, OnDestroy {
  projectSubscription: Subscription | undefined
  routerSubscription: Subscription | undefined
  currentRoute: string
  currentProject: string

  constructor( private store: Store<AppState>,
               private title: Title,
               private router: Router,
               private projectFacade: ProjectFacade,
               private errorReporter: MessageReporter ) {
    this.currentProject = ''
    this.currentRoute = ''
  }

  ngOnInit() {
    this.projectSubscription =
      this.projectFacade.GetCurrentProject$().pipe(
        tap( project => {
          if( project != null ) {
            this.currentProject = project.name
            this.updateTitle()
          }
        } )
      ).subscribe()

    this.routerSubscription =
      this.router.events.subscribe(
        ( event: NavigationEvent ) => {
          if( event instanceof NavigationStart ) {
            this.currentRoute = event.url
            this.updateTitle()
          }
        } )
  }

  private updateTitle() {
    if( (this.currentRoute.startsWith( `/issues` )) &&
      (this.currentProject.length > 0) ) {
      this.title.setTitle( `${this.currentProject} - Issues` )
    }
  }

  ngOnDestroy() {
    this.projectSubscription?.unsubscribe()
    this.routerSubscription?.unsubscribe()
  }
}
