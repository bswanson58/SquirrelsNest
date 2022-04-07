import {Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle, TextField} from '@mui/material'
import Button from '@mui/material/Button'
import React, {useEffect, useState} from 'react'
import { ClIssueEntry } from '../data/mutationEntities'

interface DialogProps {
  initialValues: ClIssueEntry
  open: boolean
  onClose: () => void
  onConfirm: (issue: ClIssueEntry) => void
}

function AddIssueDialog( props: DialogProps ) {
  const [title, setTitle] = useState<String>('')
  const [description, setDescription] = useState<String>('')

  const handleConfirm = () => {
    props.onConfirm({ title: title, description: description })
  }

  useEffect(() => {
    setTitle( props.initialValues.title )
    setDescription( props.initialValues.description )
  },[props.initialValues])

  return (
    <Dialog open={props.open} onClose={props.onClose} aria-labelledby='Add Issue'
            PaperProps={{ sx: { width: '600px', height: '500px' } }}>
      <DialogTitle>Add Issue</DialogTitle>
      <DialogContent>
        <DialogContentText>Issue Title</DialogContentText>
        <TextField
          autoFocus
          value={title}
          onChange={event => setTitle(event.currentTarget.value)}
          margin='dense'
          id='title'
          label='title'
          type='text'
          fullWidth />
        <DialogContentText>Description</DialogContentText>
        <TextField
          value={description}
          onChange={event => setDescription(event.currentTarget.value)}
          margin='dense'
          id='description'
          label='description'
          type='text'
          fullWidth />
      </DialogContent>
      <DialogActions>
        <Button onClick={() => props.onClose()} color='primary'>
          cancel
        </Button>
        <Button onClick={() => handleConfirm()} color='primary' autoFocus>
          ok
        </Button>
      </DialogActions>
    </Dialog>)
}

export default AddIssueDialog
