import {Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle, TextField} from '@mui/material'
import Button from '@mui/material/Button'
import React, {useEffect, useState} from 'react'
import {AddIssueInput} from '../../data/graphQlTypes'
import {selectCurrentProject} from '../../store/projects'
import {useAppSelector} from '../../store/storeHooks'

interface DialogProps {
  initialValues: AddIssueInput
  open: boolean
  onClose: () => void
  onConfirm: ( issue: AddIssueInput ) => void
}

function AddIssueDialog( props: DialogProps ) {
  const currentProject = useAppSelector( selectCurrentProject )
  const [title, setTitle] = useState<string>( '' )
  const [description, setDescription] = useState<string>( '' )

  const handleConfirm = () => {
    if( currentProject !== null ) {
      props.onConfirm( { title: title, description: description, projectId: currentProject.id } )
    }
  }

  useEffect( () => {
    setTitle( props.initialValues.title )
    setDescription( props.initialValues.description )
  }, [props.initialValues] )

  return (
    <Dialog open={props.open} onClose={props.onClose} aria-labelledby='Add Issue'
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
