import {Component, Inject} from '@angular/core'
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog'
import {ProjectFacade} from '../project.facade'

export interface ComponentEditData {
  name: string,
  description: string
}

export interface ComponentEditResult {
  accepted: boolean,
  name: string,
  description: string
}

@Component( {
  selector: 'sn-component-edit-dialog',
  templateUrl: './component-edit-dialog.component.html',
  styleUrls: ['./component-edit-dialog.component.css']
} )
export class ComponentEditDialogComponent {
  name: string
  description: string

  constructor( private dialogRef: MatDialogRef<ComponentEditDialogComponent>,
               @Inject( MAT_DIALOG_DATA ) private dialogData: ComponentEditData ) {
    this.name = dialogData.name
    this.description = dialogData.description

    this.dialogRef.updateSize( '600px' )
  }

  onClose() {
    const result: ComponentEditResult = {
      accepted: true,
      name: this.name,
      description: this.description,
    }

    this.dialogRef.close( result )
  }

  onCancel() {
    this.dialogRef.close( { accepted: false } as ComponentEditResult )
  }
}
