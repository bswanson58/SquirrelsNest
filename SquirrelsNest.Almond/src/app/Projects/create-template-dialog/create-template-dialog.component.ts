import {Component} from '@angular/core'
import {MatDialogRef} from '@angular/material/dialog'

export interface CreateTemplateResult {
  accepted: boolean,
  name: string,
  description: string
}

@Component( {
  selector: 'sn-create-template-dialog',
  templateUrl: './create-template-dialog.component.html',
  styleUrls: ['./create-template-dialog.component.css']
} )
export class CreateTemplateDialogComponent {
  name: string
  description: string

  constructor( private dialogRef: MatDialogRef<CreateTemplateDialogComponent> ) {
    this.name = ''
    this.description = ''

    this.dialogRef.updateSize( '600px' )
  }

  onClose() {
    const result: CreateTemplateResult = {
      accepted: true,
      name: this.name,
      description: this.description,
    }

    this.dialogRef.close( result )
  }

  onCancel() {
    this.dialogRef.close( { accepted: false } as CreateTemplateResult )
  }
}
