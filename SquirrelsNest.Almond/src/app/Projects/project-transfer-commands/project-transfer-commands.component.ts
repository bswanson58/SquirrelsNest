import {Component} from '@angular/core'
import {MatDialog, MatDialogConfig} from '@angular/material/dialog'
import {
  ProjectImportDialogComponent,
  ProjectImportResult
} from '../project-import-dialog/project-import-dialog.component'
import {ProjectFacade} from '../project.facade'
import {saveAs} from 'file-saver'

@Component( {
  selector: 'sn-project-transfer-commands',
  templateUrl: './project-transfer-commands.component.html',
  styleUrls: ['./project-transfer-commands.component.css']
} )
export class ProjectTransferCommandsComponent {

  constructor( private projectFacade: ProjectFacade, private dialog: MatDialog ) {
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

  onExportProject() {
    const project = this.projectFacade.GetCurrentProject()

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
