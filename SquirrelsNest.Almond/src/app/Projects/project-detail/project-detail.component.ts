import {Component} from '@angular/core'
import {MatDialog, MatDialogConfig} from '@angular/material/dialog'
import {Observable} from 'rxjs'
import {ClProject, UpdateProjectInput} from '../../Data/graphQlTypes'
import {
  ProjectEditData,
  ProjectEditDialogComponent,
  ProjectEditResult
} from '../project-edit-dialog/project-edit-dialog.component'
import {ProjectFacade} from '../project.facade'

@Component( {
  selector: 'sn-project-detail',
  templateUrl: './project-detail.component.html',
  styleUrls: ['./project-detail.component.css']
} )
export class ProjectDetailComponent {
  project: Observable<ClProject | null>

  constructor( private projectFacade: ProjectFacade, private dialog: MatDialog ) {
    this.project = projectFacade.GetCurrentProject$()
  }

  async onEditProject(): Promise<void> {
    const currentProject = await this.projectFacade.GetCurrentProject()

    if( currentProject !== null ) {
      const dialogData: ProjectEditData = {
        project: currentProject
      }
      const dialogConfig = new MatDialogConfig()
      dialogConfig.data = dialogData

      this.dialog
        .open( ProjectEditDialogComponent, dialogConfig )
        .afterClosed()
        .subscribe( ( result: ProjectEditResult ) => {
          if( (result.accepted) &&
            (result.project !== null) ) {
            const project: UpdateProjectInput = {
              projectId: currentProject.id,
              title: result.project?.title,
              description: result.project?.description,
              issuePrefix: result.project?.issuePrefix
            }

            this.projectFacade.UpdateProject( project )
          }
        } )
    }
  }
}
