import {Component, OnInit} from '@angular/core'
import {MatDialogRef} from '@angular/material/dialog'
import {Observable} from 'rxjs'
import {AddProjectInput, ClProjectTemplate} from '../../Data/graphQlTypes'
import {ProjectFacade} from '../project.facade'

export interface ProjectCreateResult {
  accepted: boolean
  project: AddProjectInput | null
}

@Component( {
  selector: 'sn-project-create-dialog',
  templateUrl: './project-create-dialog.component.html',
  styleUrls: ['./project-create-dialog.component.css']
} )
export class ProjectCreateDialogComponent implements OnInit {
  projectTitle: string
  projectDescription: string
  issuePrefix: string
  templateList$: Observable<ClProjectTemplate[]>
  selectedTemplate: string

  constructor( private dialogRef: MatDialogRef<ProjectCreateDialogComponent>,
               private projectFacade: ProjectFacade ) {
    this.projectTitle = ''
    this.projectDescription = ''
    this.issuePrefix = ''
    this.templateList$ = projectFacade.GetProjectTemplates$()
    this.selectedTemplate = ''

    this.dialogRef.updateSize( '600px' )
  }

  ngOnInit(): void {
    this.projectFacade.LoadProjectTemplates()
  }

  onClose() {
    const projectInput: AddProjectInput = {
      title: this.projectTitle,
      description: this.projectDescription,
      issuePrefix: this.issuePrefix,
      projectTemplate: this.selectedTemplate ?? ''
    }

    this.dialogRef.close( { accepted: true, project: projectInput } as ProjectCreateResult )
  }

  onCancel() {
    this.dialogRef.close( { accepted: false, project: null } as ProjectCreateResult )
  }
}
