import {Component, EventEmitter, OnInit, Output} from '@angular/core'
import {Store} from '@ngrx/store'
import {Observable} from 'rxjs'
import {AuthService} from '../../Auth/auth.service'
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

  constructor( private store: Store<AppState>, private authService: AuthService ) {
    this.isAuthenticated$ = new Observable<boolean>()
  }

  ngOnInit(): void {
    this.isAuthenticated$ = this.store.select( getIsAuthenticated )
  }

  onLogout() {
    this.authService.Logout()
  }

  onToggleSidenav() {
    this.sidenavToggle.emit()
  }
}
