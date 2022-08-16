import {Component} from '@angular/core'
import {ProjectFacade} from '../project.facade'

@Component( {
  selector: 'sn-project-commands',
  templateUrl: './project-commands.component.html',
  styleUrls: ['./project-commands.component.css']
} )
export class ProjectCommandsComponent {

  constructor( private projectFacade: ProjectFacade ) {
  }

  onCreateProject() {
    this.projectFacade.CreateProject()
  }

  onDeleteProject() {
    const currentProject = this.projectFacade.GetCurrentProject()

    if( currentProject !== null ) {
      this.projectFacade.DeleteProject( currentProject )
    }
  }
}
