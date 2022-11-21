import {Component} from '@angular/core'
import {MatDialog, MatDialogConfig} from '@angular/material/dialog'
import {map, Observable} from 'rxjs'
import {ClIssueType, ProjectDetailInput} from '../../Data/graphQlTypes'
import {
  IssueTypeEditData,
  IssueTypeEditDialogComponent, IssueTypeEditResult
} from '../issue-type-edit-dialog/issue-type-edit-dialog.component'
import {ProjectFacade} from '../project.facade'

@Component( {
  selector: 'sn-issue-types-list',
  templateUrl: './issue-types-list.component.html',
  styleUrls: ['./issue-types-list.component.css']
} )
export class IssueTypesListComponent {
  issueTypes$: Observable<ClIssueType[]>

  constructor( private projectFacade: ProjectFacade, private dialog: MatDialog ) {
    this.issueTypes$ = projectFacade.GetCurrentProject$()
      .pipe(
        map( project => project ? project.issueTypes : [] ),
        map( issueTypes => issueTypes.filter( t => t.id != 'default' ) )
      )
  }

  async onAddIssueType(): Promise<void> {
    const currentProject = await this.projectFacade.GetCurrentProject()

    if( currentProject !== null ) {
      const input: IssueTypeEditData = {
        name: '',
        description: ''
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
                id: '',
                projectId: currentProject.id,
                name: result.name,
                description: result.description,
              }],
              states: [],
            }

            this.projectFacade.AddProjectDetail( detail )
          }
        } )
    }
  }
}
