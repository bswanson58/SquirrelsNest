import {Component, Input} from '@angular/core'
import {MatDialog, MatDialogConfig} from '@angular/material/dialog'
import {ClWorkflowState, ProjectDetailInput, StateCategory} from '../../../Data/graphQlTypes'
import {
  ConfirmDialogComponent,
  ConfirmDialogData,
  ConfirmDialogResult
} from '../../../UI/confirm-dialog/confirm-dialog.component'
import {ProjectFacade} from '../../project.facade'
import {
  WorkflowEditDialogComponent,
  WorkflowStateEditData,
  WorkflowStateEditResult
} from '../../workflow-edit-dialog/workflow-edit-dialog.component'

@Component( {
  selector: 'sn-workflow-item',
  templateUrl: './workflow-item.component.html',
  styleUrls: ['./workflow-item.component.css']
} )
export class WorkflowItemComponent {
  @Input() workflow!: ClWorkflowState
  isHovering: boolean = false

  constructor( private projectFacade: ProjectFacade, private dialog: MatDialog ) {
  }

  async onEdit(): Promise<void> {
    const currentProject = await this.projectFacade.GetCurrentProject()

    if( currentProject !== null ) {
      const input: WorkflowStateEditData = {
        name: this.workflow.name,
        description: this.workflow.description,
        category: this.workflow.category
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
                id: this.workflow.id,
                projectId: currentProject.id,
                name: result.name,
                description: result.description ? result.description : '',
                category: result.category as StateCategory
              }],
            }

            this.projectFacade.UpdateProjectDetail( detail )
          }
        } )
    }
  }

  async onDelete(): Promise<void> {
    const project = await this.projectFacade.GetCurrentProject()

    if( project !== null ) {
      const dialogData: ConfirmDialogData = {
        prompt: 'Do you want to delete this Workflow State?',
        promptDetail: this.workflow.name
      }
      const dialogConfig = new MatDialogConfig()
      dialogConfig.data = dialogData

      this.dialog
        .open( ConfirmDialogComponent, dialogConfig )
        .afterClosed()
        .subscribe( ( result: ConfirmDialogResult ) => {
          if( result.accepted ) {
            const details: ProjectDetailInput = {
              projectId: project.id,
              components: [],
              issueTypes: [],
              states: [{
                id: this.workflow.id,
                name: this.workflow.name,
                description: this.workflow.description ? this.workflow.description : '',
                category: 'INTERMEDIATE' as StateCategory.Intermediate,
                projectId: project.id
              }],
            }

            this.projectFacade.DeleteProjectDetail( details )
          }
        } )
    }
  }
}
