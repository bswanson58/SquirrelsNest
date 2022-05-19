import {Dialog, DialogActions, DialogContent, DialogTitle, SelectChangeEvent,} from '@mui/material'
import Button from '@mui/material/Button'
import React, {PropsWithChildren, useState} from 'react'
import {UserSelector} from '../../components/UserSelector'
import {ClIssue, UpdateIssueInput} from '../../data/graphQlTypes'
import {updateIssue} from '../../store/issueMutations'
import {useAppDispatch} from '../../store/storeHooks'
import {hideModal} from '../../store/uiActions'

interface editUserProps {
  payload: {
    modalProps: ClIssue
  }
}

function EditUserDialog( props: PropsWithChildren<editUserProps> ) {
  const dispatch = useAppDispatch()
  const issue = props.payload.modalProps
  const [user, setUser] = useState<string>( issue.assignedTo.id )

  const handleCancel = () => {
    dispatch( hideModal() )
  }

  const handleConfirm = () => {
    if( (user !== undefined) &&
      (user !== issue.assignedTo.id) ) {
      const updateInfo: UpdateIssueInput = {
        issueId: issue.id,
        operations: [{
          // @ts-ignore
          path: 'ASSIGNED_TO_ID',
          value: user,
        }]
      }

      dispatch( updateIssue( updateInfo ) )
    }

    dispatch( hideModal() )
  }

  function handleComponentChange( event: SelectChangeEvent ): void {
    setUser( event.target.value )
  }

  return (
    <Dialog open={true} onClose={() => handleCancel()} aria-labelledby='Edit Assigned User'
            PaperProps={{ sx: { width: '300px', height: '200px' } }}>
      <DialogTitle>Edit Assigned User</DialogTitle>
      <DialogContent>
        <UserSelector initialValue={issue.assignedTo.id} handleChange={handleComponentChange}/>
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

export default EditUserDialog
