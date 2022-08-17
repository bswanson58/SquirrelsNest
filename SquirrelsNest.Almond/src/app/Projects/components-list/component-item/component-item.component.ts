import {Component, Input} from '@angular/core'
import {MatDialog, MatDialogConfig} from '@angular/material/dialog'
import {ClComponent, ProjectDetailInput} from '../../../Data/graphQlTypes'
import {
  ConfirmDialogComponent,
  ConfirmDialogData,
  ConfirmDialogResult
} from '../../../UI/confirm-dialog/confirm-dialog.component'
import {
  ComponentEditData,
  ComponentEditDialogComponent, ComponentEditResult
} from '../../component-edit-dialog/component-edit-dialog.component'
import {ProjectFacade} from '../../project.facade'

@Component( {
  selector: 'sn-component-item',
  templateUrl: './component-item.component.html',
  styleUrls: ['./component-item.component.css']
} )
export class ComponentItemComponent {
  @Input() component!: ClComponent
  isHovering: boolean = false

  constructor( private projectFacade: ProjectFacade, private dialog: MatDialog ) {
  }

  onEdit() {
    const currentProject = this.projectFacade.GetCurrentProject()

    if( currentProject !== null ) {
      const input: ComponentEditData = {
        name: this.component.name,
        description: this.component.description,
      }
      const dialogConfig = new MatDialogConfig()
      dialogConfig.data = input

      this.dialog
        .open( ComponentEditDialogComponent, dialogConfig )
        .afterClosed()
        .subscribe( ( result: ComponentEditResult ) => {
          if( result.accepted ) {
            const detail: ProjectDetailInput = {
              projectId: currentProject.id,
              components: [{
                id: this.component.id,
                projectId: currentProject.id,
                name: result.name,
                description: result.description ? result.description : ''
              }],
              states: [],
              issueTypes: []
            }

            this.projectFacade.UpdateProjectDetail( detail )
          }
        } )
    }
  }

  onDelete() {
    const project = this.projectFacade.GetCurrentProject()

    if( project !== null ) {
      const dialogData: ConfirmDialogData = {
        prompt: 'Do you want to delete this Component?',
        promptDetail: this.component.name
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
              components: [{
                id: this.component.id,
                name: this.component.name,
                description: this.component.description ? this.component.description : '',
                projectId: project.id
              }],
              states: [],
              issueTypes: []
            }

            this.projectFacade.DeleteProjectDetail( details )
          }
        } )
    }
  }
}
