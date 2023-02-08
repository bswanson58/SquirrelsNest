import {Component, Inject} from '@angular/core'
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog'

export interface DetailItem {
  id: string,
  name: string
}

export interface DetailSelectorData {
  issueTitle: string,
  dialogTitle: string,
  currentItem: DetailItem,
  items: DetailItem[]
}

export interface DetailSelectorResult {
  accepted: boolean
  selectedId: string
}

@Component( {
  selector: 'sn-issue-detail-selector',
  templateUrl: './issue-detail-selector.component.html',
  styleUrls: ['./issue-detail-selector.component.css']
} )
export class IssueDetailSelectorComponent {
  issueTitle: string
  dialogTitle: string
  selectorValues: DetailItem[]
  selectedItemId: string

  constructor( private dialogRef: MatDialogRef<IssueDetailSelectorComponent>,
               @Inject( MAT_DIALOG_DATA ) selectorData: DetailSelectorData ) {
    this.issueTitle = selectorData.issueTitle
    this.dialogTitle = selectorData.dialogTitle
    this.selectorValues = selectorData.items
    this.selectedItemId = selectorData.currentItem.id
  }

  onClose() {
    this.dialogRef.close( { accepted: true, selectedId: this.selectedItemId } as DetailSelectorResult )
  }

  onCancel() {
    this.dialogRef.close( { accepted: false, selectedId: '' } as DetailSelectorResult )
  }
}
