import {Injectable} from '@angular/core'
import {Store} from '@ngrx/store'
import {Observable, take} from 'rxjs'
import {ClProject} from '../Data/graphQlTypes'
import {AppState} from '../Store/app.reducer'
import {getSelectedProject} from '../Store/app.selectors'
import {ProjectService} from './projects.service'

@Injectable( {
  providedIn: 'root'
} )
export class ProjectFacade {
  constructor( private store: Store<AppState>, private service: ProjectService ) {
  }

  GetCurrentProject$(): Observable<ClProject | null> {
    return this.store.select( getSelectedProject )
  }

  GetCurrentProject(): ClProject | null {
    let currentProject: ClProject | null = null

    this.store.select( getSelectedProject ).pipe( take( 1 ) ).subscribe( project => currentProject = project )

    return currentProject
  }
}
