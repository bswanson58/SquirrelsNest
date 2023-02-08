import {Dialog, DialogActions, DialogContent, DialogTitle, SelectChangeEvent,} from '@mui/material'
import Button from '@mui/material/Button'
import React, {PropsWithChildren, useState} from 'react'
import {WorkflowSelector} from '../../components/WorkflowSelector'
import {ClIssue, UpdateIssueInput} from '../../data/graphQlTypes'
import {updateIssue} from '../../store/issueMutations'
import {useAppDispatch} from '../../store/storeHooks'
import {hideModal} from '../../store/uiActions'

interface editWorkflowProps {
  payload: {
    modalProps: ClIssue
  }
}

function EditWorkflowDialog( props: PropsWithChildren<editWorkflowProps> ) {
  const dispatch = useAppDispatch()
  const issue = props.payload.modalProps
  const [state, setState] = useState<string>( issue.workflowState.id )

  const handleCancel = () => {
    dispatch( hideModal() )
  }

  const handleConfirm = () => {
    if( (state !== undefined) &&
      (state !== issue.workflowState.id) ) {
      const updateInfo: UpdateIssueInput = {
        issueId: issue.id,
        operations: [{
          // @ts-ignore
          path: 'WORKFLOW_STATE_ID',
          value: state,
        }]
      }

      dispatch( updateIssue( updateInfo ) )
    }

    dispatch( hideModal() )
  }

  function handleWorkflowStateChange( event: SelectChangeEvent ): void {
    setState( event.target.value )
  }

  return (
    <Dialog open={true} onClose={() => handleCancel()} aria-labelledby='Edit Workflow State'
            PaperProps={{ sx: { width: '300px', height: '200px' } }}>
      <DialogTitle>Edit Workflow State</DialogTitle>
      <DialogContent>
        <WorkflowSelector initialValue={issue.workflowState.id} handleChange={handleWorkflowStateChange}/>
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

export default EditWorkflowDialog
