import {Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle, TextField} from '@mui/material'
import Button from '@mui/material/Button'
import React, {useState} from 'react'
import {addIssue} from '../../store/issueMutations'
import {selectCurrentProject} from '../../store/projects'
import {useAppDispatch, useAppSelector} from '../../store/storeHooks'
import {hideModal} from '../../store/uiActions'

function AddIssueDialog() {
  const dispatch = useAppDispatch()
  const currentProject = useAppSelector( selectCurrentProject )

  const [title, setTitle] = useState<string>( '' )
  const [description, setDescription] = useState<string>( '' )

  const handleCancel = () => {
    dispatch( hideModal() )
  }

  const handleConfirm = () => {
    if( currentProject !== null ) {
      dispatch( addIssue( { title: title, description: description, projectId: currentProject.id } ) )
    }
    dispatch( hideModal() )
  }

  return (
    <Dialog open={true} onClose={() => handleCancel()} aria-labelledby='Add Issue'
            PaperProps={{ sx: { width: '600px', height: '500px' } }}>
      <DialogTitle>Add Issue</DialogTitle>
      <DialogContent>
        <DialogContentText>Issue Title</DialogContentText>
        <TextField
          autoFocus
          value={title}
          onChange={event => setTitle( event.currentTarget.value )}
          margin='dense'
          id='title'
          label='title'
          type='text'
          fullWidth/>
        <DialogContentText>Description</DialogContentText>
        <TextField
          value={description}
          onChange={event => setDescription( event.currentTarget.value )}
          margin='dense'
          id='description'
          label='description'
          type='text'
          fullWidth/>
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

export default AddIssueDialog
