import {Component, Inject} from '@angular/core'
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog'
import {Observable} from 'rxjs'
import {
  ClComponent,
  ClIssue,
  ClIssueType,
  ClProject, ClProjectBase,
  ClUser,
  ClWorkflowState,
} from '../../Data/graphQlTypes'
import {ProjectFacade} from '../../Projects/project.facade'

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
  components$: Observable<ClComponent[]>
  issueTypes$: Observable<ClIssueType[]>
  workflowStates$: Observable<ClWorkflowState[]>
  users$: Observable<ClUser[]>

  issueTitle: string
  issueDescription: string
  selectedComponentId: string
  selectedIssueTypeId: string
  selectedWorkflowId: string
  selectedUserId: string

  constructor( private dialogRef: MatDialogRef<IssueEditDialogComponent>,
               @Inject( MAT_DIALOG_DATA ) private dialogData: IssueEditData,
               private projectFacade: ProjectFacade ) {
    this.issueTitle = this.dialogData.issue !== null ? this.dialogData.issue.title : ''
    this.issueDescription = this.dialogData.issue !== null ? this.dialogData.issue?.description : ''

    this.components$ = this.projectFacade.GetCurrentProjectComponents$()
    this.issueTypes$ = this.projectFacade.GetCurrentProjectIssueTypes$()
    this.workflowStates$ = this.projectFacade.GetCurrentProjectWorkflowStates$()
    this.users$ = this.projectFacade.GetCurrentProjectUsers$()

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
