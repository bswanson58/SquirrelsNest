import {Component} from '@angular/core'
import {MatDialog, MatDialogConfig} from '@angular/material/dialog'
import {saveAs} from 'file-saver'
import {Observable} from 'rxjs'
import {ClProject, CreateTemplateInput} from '../../Data/graphQlTypes'
import {
  CreateTemplateDialogComponent,
  CreateTemplateResult
} from '../create-template-dialog/create-template-dialog.component'
import {
  ProjectImportDialogComponent,
  ProjectImportResult
} from '../project-import-dialog/project-import-dialog.component'
import {ProjectFacade} from '../project.facade'

@Component( {
  selector: 'sn-project-commands',
  templateUrl: './project-commands.component.html',
  styleUrls: ['./project-commands.component.css']
} )
export class ProjectCommandsComponent {
  project$: Observable<ClProject | null>

  constructor( private projectFacade: ProjectFacade, private dialog: MatDialog ) {
    this.project$ = projectFacade.GetCurrentProject$()
  }

  onCreateProject() {
    this.projectFacade.CreateProject()
  }

  async onCreateTemplate(): Promise<void> {
    const currentProject = await this.projectFacade.GetCurrentProject()

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

  async onDeleteProject(): Promise<void> {
    const currentProject = await this.projectFacade.GetCurrentProject()

    if( currentProject !== null ) {
      this.projectFacade.DeleteProject( currentProject )
    }
  }

  onImportProject() {
    const dialogConfig = new MatDialogConfig()

    this.dialog
      .open( ProjectImportDialogComponent, dialogConfig )
      .afterClosed()
      .subscribe( ( result: ProjectImportResult ) => {
        if( result.accepted ) {
          const formData = new FormData()

          formData.append( 'file', result.importFile )

          this.projectFacade.UploadProject( result.projectName, formData ).subscribe()
        }
      } )
  }

  async onExportProject(): Promise<void> {
    const project = await this.projectFacade.GetCurrentProject()

    if( project !== null ) {
      this.projectFacade.DownloadProject( project )
        .subscribe( ( response ) => {
          const today = (new Date()).toLocaleDateString().replaceAll( '/', '-' )
          const fileName = `${project.name} project export on ${today}.json`

          saveAs( response, fileName )
        } )
    }
  }
}
