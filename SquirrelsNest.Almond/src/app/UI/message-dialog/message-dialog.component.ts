import {Component, Inject} from '@angular/core'
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog'

export interface MessageInput {
  title: string,
  message: string
}

@Component( {
  selector: 'app-message-dialog',
  templateUrl: './message-dialog.component.html',
  styleUrls: ['./message-dialog.component.css']
} )
export class MessageDialogComponent {
  title: string
  message: string

  constructor( private dialogRef: MatDialogRef<MessageDialogComponent>,
               @Inject( MAT_DIALOG_DATA ) private dialogData: MessageInput ) {
    this.title = dialogData.title
    this.message = dialogData.message
  }

  onClose() {
    this.dialogRef.close()
  }
}
