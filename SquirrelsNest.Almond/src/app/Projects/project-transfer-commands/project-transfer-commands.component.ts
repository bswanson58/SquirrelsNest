import {Component} from '@angular/core'
import {ProjectFacade} from '../project.facade'

@Component( {
  selector: 'sn-project-transfer-commands',
  templateUrl: './project-transfer-commands.component.html',
  styleUrls: ['./project-transfer-commands.component.css']
} )
export class ProjectTransferCommandsComponent {

  constructor( private projectFacade: ProjectFacade ) {
  }

  onImportProject() {
    if( this.projectFacade.GetCurrentProject() !== null ) {
    }
  }

  onExportProject() {
    if( this.projectFacade.GetCurrentProject() !== null ) {
    }
  }
}
