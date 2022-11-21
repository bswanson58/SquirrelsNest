import {Component} from '@angular/core'
import {MatDialog, MatDialogConfig} from '@angular/material/dialog'
import {map, Observable} from 'rxjs'
import {ProjectDetailInput, ClComponent} from '../../Data/graphQlTypes'
import {
  ComponentEditData,
  ComponentEditDialogComponent,
  ComponentEditResult
} from '../component-edit-dialog/component-edit-dialog.component'
import {ProjectFacade} from '../project.facade'

@Component( {
  selector: 'sn-components-list',
  templateUrl: './components-list.component.html',
  styleUrls: ['./components-list.component.css']
} )
export class ComponentsListComponent {
  components$: Observable<ClComponent[]>
  isHovering: boolean = false

  constructor( private projectFacade: ProjectFacade, private dialog: MatDialog ) {
    this.components$ = projectFacade.GetCurrentProject$()
      .pipe(
        map( project => project ? project.components : [] ),
        map( components => components.filter( c => c.id != 'default' ) )
      )
  }

  async onAddComponent(): Promise<void> {
    const currentProject = await this.projectFacade.GetCurrentProject()

    if( currentProject !== null ) {
      const input: ComponentEditData = {
        name: '',
        description: ''
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
                id: '',
                projectId: currentProject.id,
                name: result.name,
                description: result.description,
              }],
              states: [],
              issueTypes: []
            }

            this.projectFacade.AddProjectDetail( detail )
          }
        } )
    }
  }
}
