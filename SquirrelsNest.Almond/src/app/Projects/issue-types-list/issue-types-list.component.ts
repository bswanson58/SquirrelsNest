import {Component} from '@angular/core'
import {map, Observable} from 'rxjs'
import {ClIssueType} from '../../Data/graphQlTypes'
import {ProjectFacade} from '../project.facade'

@Component( {
  selector: 'sn-issue-types-list',
  templateUrl: './issue-types-list.component.html',
  styleUrls: ['./issue-types-list.component.css']
} )
export class IssueTypesListComponent {
  issueTypes$: Observable<ClIssueType[]>

  constructor( private projectFacade: ProjectFacade ) {
    this.issueTypes$ = projectFacade.GetCurrentProject$()
      .pipe(
        map( project => {
          return project ? project.issueTypes : []
        } ) )
  }
}
