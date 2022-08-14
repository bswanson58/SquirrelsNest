import {Component} from '@angular/core'
import {map, Observable} from 'rxjs'
import {ClComponent} from '../../Data/graphQlTypes'
import {ProjectFacade} from '../project.facade'

@Component( {
  selector: 'sn-components-list',
  templateUrl: './components-list.component.html',
  styleUrls: ['./components-list.component.css']
} )
export class ComponentsListComponent {
  components$: Observable<ClComponent[]>

  constructor( private projectFacade: ProjectFacade ) {
    this.components$ = projectFacade.GetCurrentProject$()
      .pipe(
        map( project => {
          return project ? project.components : []
        } ) )
  }
}
