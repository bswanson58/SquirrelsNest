import {Component} from '@angular/core'
import {map, Observable} from 'rxjs'
import {ClUser} from '../../Data/graphQlTypes'
import {ProjectFacade} from '../project.facade'

@Component( {
  selector: 'sn-project-users-list',
  templateUrl: './project-users-list.component.html',
  styleUrls: ['./project-users-list.component.css']
} )
export class ProjectUsersListComponent {
  projectUsers$: Observable<ClUser[]>

  constructor( projectFacade: ProjectFacade ) {
    this.projectUsers$ = projectFacade.GetCurrentProject$()
      .pipe(
        map( project => project ? project.users : [] ),
        map( users => users.filter( u => u.id != 'default' ) ),
      )
  }

}
