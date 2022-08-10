import {Component, Inject} from '@angular/core'
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog'
import {
  ClComponent,
  ClIssue,
  ClIssueType,
  ClProject, ClProjectBase,
  ClUser,
  ClWorkflowState,
} from '../../Data/graphQlTypes'

export interface IssueEditData {
  issue: ClIssue | null,
  project: ClProject
}

export interface IssueEditResult {
  accepted: boolean
  issue: ClIssue | null
}

@Component( {
  selector: 'sn-issue-edit-dialog',
  templateUrl: './issue-edit-dialog.component.html',
  styleUrls: ['./issue-edit-dialog.component.css']
} )
export class IssueEditDialogComponent {
  issueTitle: string
  issueDescription: string
  components: ClComponent[]
  selectedComponentId: string
  issueTypes: ClIssueType[]
  selectedIssueTypeId: string
  workflowStates: ClWorkflowState[]
  selectedWorkflowId: string
  users: ClUser[]
  selectedUserId: string

  constructor( private dialogRef: MatDialogRef<IssueEditDialogComponent>,
               @Inject( MAT_DIALOG_DATA ) private dialogData: IssueEditData ) {
    this.issueTitle = dialogData.issue !== null ? dialogData.issue.title : ''
    this.issueDescription = dialogData.issue !== null ? dialogData.issue?.description : ''
    this.components = dialogData.project.components
    this.issueTypes = dialogData.project.issueTypes
    this.workflowStates = dialogData.project.workflowStates
    this.users = dialogData.project.users

    this.selectedComponentId = ''
    this.selectedIssueTypeId = ''
    const initialState = dialogData.project.workflowStates.find( s => s.category === 'INITIAL' )
    this.selectedWorkflowId = initialState ? initialState.id : ''
    this.selectedUserId = ''

    this.dialogRef.updateSize( '650px' )
  }

  onClose() {
    const newIssue = {
      title: this.issueTitle,
      description: this.issueDescription,
      component: this.dialogData.project.components.find( c => c.id === this.selectedComponentId ),
      issueType: this.dialogData.project.issueTypes.find( it => it.id === this.selectedIssueTypeId ),
      workflowState: this.dialogData.project.workflowStates.find( s => s.id === this.selectedWorkflowId ),
      assignedTo: this.dialogData.project.users.find( u => u.id === this.selectedUserId ),
      project: this.dialogData.project as ClProjectBase
    } as ClIssue

    console.log( newIssue )
    this.dialogRef.close( { accepted: true, issue: newIssue } as IssueEditResult )
  }

  onCancel() {
    this.dialogRef.close( { accepted: false, issue: null } as IssueEditResult )
  }
}
