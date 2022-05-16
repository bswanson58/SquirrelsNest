import {Dialog, DialogActions, DialogContent, DialogTitle} from '@mui/material'
import Typography from '@mui/material/Typography'
import Button from '@mui/material/Button'
import React, {PropsWithChildren} from 'react'
import {ClIssue, DeleteIssueInput} from '../../data/graphQlTypes'
import {deleteIssue} from '../../store/issueMutations'
import {useAppDispatch} from '../../store/storeHooks'
import {hideModal} from '../../store/uiActions'

interface deleteIssueProps {
  payload: {
    modalProps: ClIssue
  }
}

function DeleteIssueDialog( props: PropsWithChildren<deleteIssueProps> ) {
  const dispatch = useAppDispatch()
  const issue = props.payload.modalProps

  const handleCancel = () => {
    dispatch( hideModal() )
  }

  const handleConfirm = () => {
    const deleteInfo: DeleteIssueInput = {
      issueId: issue.id
    }

    dispatch( deleteIssue( deleteInfo ) )
    dispatch( hideModal() )
  }

  return (
    <Dialog open={true} onClose={() => handleCancel()} aria-labelledby='Add Issue'
            PaperProps={{ sx: { width: '475px', height: '190px' } }}>
      <DialogTitle>Confirm Issue Deletion</DialogTitle>
      <DialogContent>
        <Typography>Delete the issue titled:</Typography>
        <Typography>{issue.title}</Typography>
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

export default DeleteIssueDialog
