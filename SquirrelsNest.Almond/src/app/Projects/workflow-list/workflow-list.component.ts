import {Component} from '@angular/core'
import {map, Observable} from 'rxjs'
import {ClWorkflowState} from '../../Data/graphQlTypes'
import {ProjectFacade} from '../project.facade'

@Component( {
  selector: 'sn-workflow-list',
  templateUrl: './workflow-list.component.html',
  styleUrls: ['./workflow-list.component.css']
} )
export class WorkflowListComponent {
  workflowStates$: Observable<ClWorkflowState[]>

  constructor( private projectFacade: ProjectFacade ) {
    this.workflowStates$ = projectFacade.GetCurrentProject$()
      .pipe(
        map( project => {
          return project ? project.workflowStates : []
        } ) )
  }
}
