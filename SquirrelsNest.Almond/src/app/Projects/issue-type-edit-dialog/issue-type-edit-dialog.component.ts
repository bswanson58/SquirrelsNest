import {Component, Inject} from '@angular/core'
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog'

export interface IssueTypeEditData {
  name: string,
  description: string
}

export interface IssueTypeEditResult {
  accepted: boolean,
  name: string,
  description: string
}

@Component({
  selector: 'sn-issue-type-edit-dialog',
  templateUrl: './issue-type-edit-dialog.component.html',
  styleUrls: ['./issue-type-edit-dialog.component.css']
})
export class IssueTypeEditDialogComponent {
  name: string
  description: string

  constructor( private dialogRef: MatDialogRef<IssueTypeEditDialogComponent>,
               @Inject( MAT_DIALOG_DATA ) private dialogData: IssueTypeEditData ) {
    this.name = dialogData.name
    this.description = dialogData.description

    this.dialogRef.updateSize( '600px' )
  }

  onClose() {
    const result: IssueTypeEditResult = {
      accepted: true,
      name: this.name,
      description: this.description,
    }

    this.dialogRef.close( result )
  }

  onCancel() {
    this.dialogRef.close( { accepted: false } as IssueTypeEditResult )
  }

}
