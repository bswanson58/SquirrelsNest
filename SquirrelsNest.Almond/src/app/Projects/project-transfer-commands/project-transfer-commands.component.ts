import {Component} from '@angular/core'
import {ProjectFacade} from '../project.facade'
import {saveAs} from 'file-saver'

@Component( {
  selector: 'sn-project-transfer-commands',
  templateUrl: './project-transfer-commands.component.html',
  styleUrls: ['./project-transfer-commands.component.css']
} )
export class ProjectTransferCommandsComponent {

  constructor( private projectFacade: ProjectFacade ) {
  }

  onImportProject( $event: Event ) {
    const element = $event.target as HTMLInputElement

    if( (element.files !== null) &&
      (element.files[0] !== null) ) {
      const formData = new FormData()

      formData.append( 'file', element.files[0] )

      this.projectFacade.UploadProject( 'New Project', formData ).subscribe()
    }
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
