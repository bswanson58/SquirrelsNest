import {Component, EventEmitter, OnInit, Output} from '@angular/core'
import {Router} from '@angular/router'
import {Store} from '@ngrx/store'
import {Observable} from 'rxjs'
import {AuthFacade} from '../../Auth/auth.facade'
import {AppState} from '../../Store/app.reducer'
import {getIsAuthenticated} from '../../Store/app.selectors'

@Component( {
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
} )
export class HeaderComponent implements OnInit {
  isAuthenticated$: Observable<boolean>

  @Output()
  sidenavToggle = new EventEmitter<void>()

  constructor( private store: Store<AppState>, private authFacade: AuthFacade, private router: Router ) {
    this.isAuthenticated$ = new Observable<boolean>()
  }

  ngOnInit(): void {
    this.isAuthenticated$ = this.store.select( getIsAuthenticated )
  }

  isActiveRoute( route: string ): boolean {
    return this.router.url.startsWith( route )
  }

  onLogout() {
    this.authFacade.Logout()
  }

  onToggleSidenav() {
    this.sidenavToggle.emit()
  }
}
