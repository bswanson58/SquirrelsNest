import {Component, Inject} from '@angular/core'
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog'
import {Observable} from 'rxjs'
import {StateCategory} from '../../Data/graphQlTypes'
import {CategoryValues} from '../project.const'
import {ProjectFacade} from '../project.facade'

export interface WorkflowStateEditData {
  name: string,
  description: string,
  category: string
}

export interface WorkflowStateEditResult {
  accepted: boolean,
  name: string,
  description: string,
  category: string
}

@Component( {
  selector: 'sn-workflow-edit-dialog',
  templateUrl: './workflow-edit-dialog.component.html',
  styleUrls: ['./workflow-edit-dialog.component.css']
} )
export class WorkflowEditDialogComponent {
  name: string
  description: string
  category: string
  categoryValues$: Observable<CategoryValues[]>

  constructor( private dialogRef: MatDialogRef<WorkflowEditDialogComponent>,
               @Inject( MAT_DIALOG_DATA ) private dialogData: WorkflowStateEditData,
               projectFacade: ProjectFacade ) {
    this.categoryValues$ = projectFacade.GetWorkflowStateCategoryValues$()
    this.name = dialogData.name
    this.description = dialogData.description
    this.category = dialogData.category

    if( dialogData.category === '' ) {
      const initialCategory = projectFacade.GetWorkflowStateCategoryValues().find( c => c.value === 'INTERMEDIATE' as StateCategory )

      if( initialCategory !== undefined ) {
        this.category = initialCategory.value
      }
    }

    this.dialogRef.updateSize( '600px' )
  }

  onClose() {
    const result: WorkflowStateEditResult = {
      accepted: true,
      name: this.name,
      description: this.description,
      category: this.category
    }

    this.dialogRef.close( result )
  }

  onCancel() {
    this.dialogRef.close( { accepted: false } as WorkflowStateEditResult )
  }
}
