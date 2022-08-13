import {Component, Inject} from '@angular/core'
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog'

export interface ConfirmDialogData {
  prompt: string
  promptDetail: string
}

export interface ConfirmDialogResult {
  accepted: boolean
}

@Component({
  selector: 'sn-confirm-dialog',
  templateUrl: './confirm-dialog.component.html',
  styleUrls: ['./confirm-dialog.component.css']
})
export class ConfirmDialogComponent {
  prompt: string
  promptDetail: string

  constructor( private dialogRef: MatDialogRef<ConfirmDialogComponent>,
               @Inject( MAT_DIALOG_DATA ) private dialogData: ConfirmDialogData ) {
    this.prompt = dialogData.prompt
    this.promptDetail = dialogData.promptDetail
  }

  onConfirm() {
    this.dialogRef.close( { accepted: true } as ConfirmDialogResult )
  }

  onCancel() {
    this.dialogRef.close( { accepted: false } as ConfirmDialogResult )
  }
}
