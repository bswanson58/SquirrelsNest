import {Component, Input} from '@angular/core'
import {MatDialog, MatDialogConfig} from '@angular/material/dialog'
import {ClIssueType, ProjectDetailInput} from '../../../Data/graphQlTypes'
import {
  ConfirmDialogComponent,
  ConfirmDialogData,
  ConfirmDialogResult
} from '../../../UI/confirm-dialog/confirm-dialog.component'
import {
  IssueTypeEditData,
  IssueTypeEditDialogComponent, IssueTypeEditResult
} from '../../issue-type-edit-dialog/issue-type-edit-dialog.component'
import {ProjectFacade} from '../../project.facade'

@Component( {
  selector: 'sn-issue-type-item',
  templateUrl: './issue-type-item.component.html',
  styleUrls: ['./issue-type-item.component.css']
} )
export class IssueTypeItemComponent {
  @Input() issueType!: ClIssueType
  isHovering: boolean = false

  constructor( private projectFacade: ProjectFacade, private dialog: MatDialog ) {
  }

  async onEdit(): Promise<void> {
    const currentProject = await this.projectFacade.GetCurrentProject()

    if( currentProject !== null ) {
      const input: IssueTypeEditData = {
        name: this.issueType.name,
        description: this.issueType.description,
      }
      const dialogConfig = new MatDialogConfig()
      dialogConfig.data = input

      this.dialog
        .open( IssueTypeEditDialogComponent, dialogConfig )
        .afterClosed()
        .subscribe( ( result: IssueTypeEditResult ) => {
          if( result.accepted ) {
            const detail: ProjectDetailInput = {
              projectId: currentProject.id,
              components: [],
              issueTypes: [{
                id: this.issueType.id,
                projectId: currentProject.id,
                name: result.name,
                description: result.description ? result.description : ''
              }],
              states: [],
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
        prompt: 'Do you want to delete this Issue Type?',
        promptDetail: this.issueType.name
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
              issueTypes: [{
                id: this.issueType.id,
                name: this.issueType.name,
                description: this.issueType.description ? this.issueType.description : '',
                projectId: project.id
              }],
              states: [],
            }

            this.projectFacade.DeleteProjectDetail( details )
          }
        } )
    }
  }
}
