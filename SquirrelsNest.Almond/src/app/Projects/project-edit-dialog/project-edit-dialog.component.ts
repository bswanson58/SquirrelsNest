import {Component, Inject, OnInit} from '@angular/core'
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog'
import {AddProjectInput, ClProject} from '../../Data/graphQlTypes'

export interface ProjectEditData {
  project: ClProject | null
}

export interface ProjectEditResult {
  accepted: boolean
  project: AddProjectInput | null
}

@Component( {
  selector: 'sn-project-edit-dialog',
  templateUrl: './project-edit-dialog.component.html',
  styleUrls: ['./project-edit-dialog.component.css']
} )
export class ProjectEditDialogComponent implements OnInit {
  projectTitle: string
  projectDescription: string
  issuePrefix: string

  constructor(private dialogRef: MatDialogRef<ProjectEditDialogComponent>,
              @Inject( MAT_DIALOG_DATA ) private dialogData: ProjectEditData) {
    this.projectTitle = ''
    this.projectDescription = ''
    this.issuePrefix = ''

    this.dialogRef.updateSize( '600px' )
  }

  ngOnInit(): void {
    if( this.dialogData.project !== null) {
      this.projectTitle = this.dialogData.project.name
      this.projectDescription = this.dialogData.project.description
      this.issuePrefix = this.dialogData.project.issuePrefix
    }
  }

  onClose() {
    const projectInput: AddProjectInput = {
      title: this.projectTitle,
      description: this.projectDescription,
      issuePrefix: this.issuePrefix,
      projectTemplate: ''
    }

    this.dialogRef.close( { accepted: true, project: projectInput } as ProjectEditResult )
  }

  onCancel() {
    this.dialogRef.close( { accepted: false, project: null } as ProjectEditResult )
  }
}
