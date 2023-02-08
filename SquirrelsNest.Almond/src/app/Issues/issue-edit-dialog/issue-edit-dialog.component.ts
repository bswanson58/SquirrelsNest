import {Component, Inject, OnInit} from '@angular/core'
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog'
import {firstValueFrom, Observable} from 'rxjs'
import {
  ClComponent,
  ClIssue,
  ClIssueType,
  ClProject,
  ClProjectBase,
  ClUser,
  ClWorkflowState,
  StateCategory,
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
export class IssueEditDialogComponent implements OnInit {
  components$: Observable<ClComponent[]>
  issueTypes$: Observable<ClIssueType[]>
  workflowStates$: Observable<ClWorkflowState[]>
  users$: Observable<ClUser[]>

  issueTitle: string
  issueDescription: string
  selectedComponentId: string | undefined
  selectedIssueTypeId: string | undefined
  selectedWorkflowId: string | undefined
  selectedUserId: string | undefined

  constructor( private dialogRef: MatDialogRef<IssueEditDialogComponent>,
               @Inject( MAT_DIALOG_DATA ) private dialogData: IssueEditData,
               private projectFacade: ProjectFacade ) {
    this.issueTitle = this.dialogData.issue !== null ? this.dialogData.issue.title : ''
    this.issueDescription = this.dialogData.issue !== null ? this.dialogData.issue?.description : ''

    this.components$ = this.projectFacade.GetCurrentProjectComponents$()
    this.issueTypes$ = this.projectFacade.GetCurrentProjectIssueTypes$()
    this.workflowStates$ = this.projectFacade.GetCurrentProjectWorkflowStates$()
    this.users$ = this.projectFacade.GetCurrentProjectUsers$()

    this.selectedComponentId = dialogData.project.components.find( c => c.id === dialogData.issue?.component?.id )?.id ?? ''
    this.selectedIssueTypeId = dialogData.project.issueTypes.find( i => i.id === dialogData.issue?.issueType?.id )?.id ?? ''
    this.selectedWorkflowId = dialogData.project.workflowStates.find( s => s.id === dialogData.issue?.workflowState?.id )?.id ?? ''
    this.selectedUserId = dialogData.project.users.find( u => u.id === dialogData.issue?.assignedTo?.id )?.id ?? ''

    this.dialogRef.updateSize( '650px' )
  }

  async ngOnInit() {
    if( this.selectedWorkflowId === '' ) {
      const states = await firstValueFrom( this.projectFacade.GetCurrentProjectWorkflowStates$() ).then()

      this.selectedWorkflowId = states.find( s => s.category === 'INITIAL' as StateCategory.Initial )?.id ?? ''
    }
  }

  onClose() {
    const newIssue = {
      id: this.dialogData.issue?.id,
      title: this.issueTitle ?? '',
      description: this.issueDescription ?? '',
      component: this.dialogData.project.components.find( c => c.id === this.selectedComponentId ),
      issueType: this.dialogData.project.issueTypes.find( it => it.id === this.selectedIssueTypeId ),
      workflowState: this.dialogData.project.workflowStates.find( s => s.id === this.selectedWorkflowId ),
      assignedTo: this.dialogData.project.users.find( u => u.id === this.selectedUserId ),
      project: this.dialogData.project as ClProjectBase
    } as ClIssue

    this.dialogRef.close( { accepted: true, issue: newIssue } as IssueEditResult )
  }

  onCancel() {
    this.dialogRef.close( { accepted: false, issue: null } as IssueEditResult )
  }
}
