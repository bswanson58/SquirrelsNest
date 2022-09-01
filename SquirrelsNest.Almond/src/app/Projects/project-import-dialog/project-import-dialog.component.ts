import {Component} from '@angular/core'
import {MatDialogRef} from '@angular/material/dialog'

export interface ProjectImportResult {
  accepted: boolean,
  projectName: string,
  importFile: File
}

@Component( {
  selector: 'sn-project-import-dialog',
  templateUrl: './project-import-dialog.component.html',
  styleUrls: ['./project-import-dialog.component.css']
} )
export class ProjectImportDialogComponent {
  projectName: string
  importFileName: string
  importFile: File | null

  constructor( private dialogRef: MatDialogRef<ProjectImportDialogComponent> ) {
    this.projectName = ''
    this.importFile = null
    this.importFileName = ''

    this.dialogRef.updateSize( '500px' )
  }

  onFileSelected( $event: Event ) {
    const element = $event.target as HTMLInputElement

    if( (element.files !== null) &&
      (element.files[0] !== null) ) {
      this.importFile = element.files[0]
      this.importFileName = this.importFile.name
    }
  }

  onClose() {
    if( this.importFile !== null ) {
      const result: ProjectImportResult = {
        accepted: true,
        projectName: this.projectName,
        importFile: this.importFile
      }

      this.dialogRef.close( result )
    }
    else {
      this.onCancel()
    }
  }

  onCancel() {
    this.dialogRef.close( { accepted: false } as ProjectImportResult )
  }
}
