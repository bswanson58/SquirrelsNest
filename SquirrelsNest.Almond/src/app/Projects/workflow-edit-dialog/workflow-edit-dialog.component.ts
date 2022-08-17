import {Component, Inject} from '@angular/core'
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog'

export interface WorkflowStateEditData {
  name: string,
  description: string
}

export interface WorkflowStateEditResult {
  accepted: boolean,
  name: string,
  description: string
}

@Component( {
  selector: 'sn-workflow-edit-dialog',
  templateUrl: './workflow-edit-dialog.component.html',
  styleUrls: ['./workflow-edit-dialog.component.css']
} )
export class WorkflowEditDialogComponent {
  name: string
  description: string

  constructor( private dialogRef: MatDialogRef<WorkflowEditDialogComponent>,
               @Inject( MAT_DIALOG_DATA ) private dialogData: WorkflowStateEditData ) {
    this.name = dialogData.name
    this.description = dialogData.description

    this.dialogRef.updateSize( '600px' )
  }

  onClose() {
    const result: WorkflowStateEditResult = {
      accepted: true,
      name: this.name,
      description: this.description,
    }

    this.dialogRef.close( result )
  }

  onCancel() {
    this.dialogRef.close( { accepted: false } as WorkflowStateEditResult )
  }
}
