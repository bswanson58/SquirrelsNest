import {
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  SelectChangeEvent,
} from '@mui/material'
import Button from '@mui/material/Button'
import React, {PropsWithChildren, useState} from 'react'
import {IssueTypeSelector} from '../../components/IssueTypeSelector'
import {ClIssue, EditIssueInput} from '../../data/graphQlTypes'
import {editIssue} from '../../store/issueMutations'
import {useAppDispatch} from '../../store/storeHooks'
import {hideModal} from '../../store/uiActions'

interface editIssueProps {
  payload: {
    modalProps: ClIssue
  }
}

function EditIssueTypeDialog( props: PropsWithChildren<editIssueProps> ) {
  const dispatch = useAppDispatch()
  const issue = props.payload.modalProps
  const [issueType, setIssueType] = useState<string>( issue.issueType.id )

  const handleCancel = () => {
    dispatch( hideModal() )
  }

  const handleConfirm = () => {
    if( (issueType !== undefined) &&
      (issueType !== issue.issueType.id) ) {
      const issueInput: EditIssueInput = {
        assignedToId: issue.assignedTo.id ? issue.assignedTo.id : '',
        componentId: issue.component.id ? issue.component.id : '',
        description: issue.description,
        issueId: issue.id,
        issueTypeId: issueType,
        releaseId: '',
        title: issue.title,
        workflowStateId: issue.workflowState.id ? issue.workflowState.id : '',
      }

      console.log( `update issue type to ${issueType}` )

      dispatch( editIssue( issueInput ) )
    }

    dispatch( hideModal() )
  }

  function handleIssueTypeChange( event: SelectChangeEvent ): void {
    setIssueType( event.target.value )
  }

  return (
    <Dialog open={true} onClose={() => handleCancel()} aria-labelledby='Add Issue'
            PaperProps={{ sx: { width: '300px', height: '200px' } }}>
      <DialogTitle>Edit Issue Type</DialogTitle>
      <DialogContent>
        <IssueTypeSelector initialValue={issue.issueType.id} handleChange={handleIssueTypeChange}/>
      </DialogContent>
      <DialogActions>
        <Button onClick={() => handleCancel()} color='primary'>
          cancel
        </Button>
        <Button onClick={() => handleConfirm()} color='primary' autoFocus>
          ok
        </Button>
      </DialogActions>
    </Dialog>)
}

export default EditIssueTypeDialog
