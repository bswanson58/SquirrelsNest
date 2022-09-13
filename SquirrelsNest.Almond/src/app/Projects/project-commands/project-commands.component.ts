import {Component} from '@angular/core'
import {MatDialog} from '@angular/material/dialog'
import {CreateTemplateInput} from '../../Data/graphQlTypes'
import {
  CreateTemplateDialogComponent,
  CreateTemplateResult
} from '../create-template-dialog/create-template-dialog.component'
import {ProjectFacade} from '../project.facade'

@Component( {
  selector: 'sn-project-commands',
  templateUrl: './project-commands.component.html',
  styleUrls: ['./project-commands.component.css']
} )
export class ProjectCommandsComponent {

  constructor( private projectFacade: ProjectFacade, private dialog: MatDialog ) {
  }

  onCreateProject() {
    this.projectFacade.CreateProject()
  }

  onCreateTemplate() {
    const currentProject = this.projectFacade.GetCurrentProject()

    if( currentProject !== null ) {
      this.dialog
        .open( CreateTemplateDialogComponent )
        .afterClosed()
        .subscribe( ( result: CreateTemplateResult ) => {
          if( result.accepted ) {
            const templateInput: CreateTemplateInput = {
              projectId: currentProject.id,
              name: result.name,
              description: result.description
            }

            this.projectFacade.CreateProjectTemplate( templateInput )
          }
        } )
    }
  }

  onDeleteProject() {
    const currentProject = this.projectFacade.GetCurrentProject()

    if( currentProject !== null ) {
      this.projectFacade.DeleteProject( currentProject )
    }
  }
}
