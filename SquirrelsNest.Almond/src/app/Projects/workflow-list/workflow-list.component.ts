import {Component} from '@angular/core'
import {MatDialog, MatDialogConfig} from '@angular/material/dialog'
import {map, Observable} from 'rxjs'
import {ClWorkflowState, ProjectDetailInput, StateCategory} from '../../Data/graphQlTypes'
import {ProjectFacade} from '../project.facade'
import {
  WorkflowEditDialogComponent,
  WorkflowStateEditData, WorkflowStateEditResult
} from '../workflow-edit-dialog/workflow-edit-dialog.component'

@Component( {
  selector: 'sn-workflow-list',
  templateUrl: './workflow-list.component.html',
  styleUrls: ['./workflow-list.component.css']
} )
export class WorkflowListComponent {
  workflowStates$: Observable<ClWorkflowState[]>

  constructor( private projectFacade: ProjectFacade, private dialog: MatDialog ) {
    this.workflowStates$ = projectFacade.GetCurrentProject$()
      .pipe(
        map( project => {
          return project ? project.workflowStates : []
        } ) )
  }

  onAddWorkflowState() {
    const currentProject = this.projectFacade.GetCurrentProject()

    if( currentProject !== null ) {
      const input: WorkflowStateEditData = {
        name: '',
        description: '',
        category: ''
      }
      const dialogConfig = new MatDialogConfig()
      dialogConfig.data = input

      this.dialog
        .open( WorkflowEditDialogComponent, dialogConfig )
        .afterClosed()
        .subscribe( ( result: WorkflowStateEditResult ) => {
          if( result.accepted ) {
            const detail: ProjectDetailInput = {
              projectId: currentProject.id,
              components: [],
              issueTypes: [],
              states: [{
                id: '',
                projectId: currentProject.id,
                name: result.name,
                description: result.description,
                category: result.category as StateCategory
              }],
            }

            this.projectFacade.AddProjectDetail( detail )
          }
        } )
    }
  }
}
