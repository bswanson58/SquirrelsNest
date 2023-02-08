import {
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  FormControl,
  SelectChangeEvent,
  TextField
} from '@mui/material'
import Button from '@mui/material/Button'
import React, {useState} from 'react'
import {IssueTypeSelector} from '../../components/IssueTypeSelector'
import {addIssue} from '../../store/issueMutations'
import {selectCurrentProject} from '../../store/projects'
import {useAppDispatch, useAppSelector} from '../../store/storeHooks'
import {hideModal} from '../../store/uiActions'

function AddIssueDialog() {
  const dispatch = useAppDispatch()
  const currentProject = useAppSelector( selectCurrentProject )

  const [title, setTitle] = useState<string>( '' )
  const [issueType, setIssueType] = useState<string>( '' )
  const [description, setDescription] = useState<string>( '' )

  const handleCancel = () => {
    dispatch( hideModal() )
  }

  const handleConfirm = () => {
    if( currentProject !== null ) {
      dispatch( addIssue( {
        title: title,
        description: description,
        issueTypeId: issueType,
        projectId: currentProject.id
      } ) )
    }
    dispatch( hideModal() )
  }

  function handleIssueTypeChange( event: SelectChangeEvent ): void {
    setIssueType( event.target.value )
  }

  return (
    <Dialog open={true} onClose={() => handleCancel()} aria-labelledby='Add Issue'
            PaperProps={{ sx: { width: '600px', height: '500px' } }}>
      <DialogTitle>Add Issue</DialogTitle>
      <DialogContent>
        <FormControl variant='standard'>
          <DialogContentText>Issue Title:</DialogContentText>
          <TextField
            id='add-issue-name-field'
            variant='outlined'
            size='small'
            label={title ? '' : 'required'}
            required
            autoFocus
            value={title}
            onChange={event => setTitle( event.currentTarget.value )}
            margin='dense'
            type='text'
            fullWidth/>
        </FormControl>
        <DialogContentText>Description:</DialogContentText>
        <TextField
          size='small'
          value={description}
          onChange={event => setDescription( event.currentTarget.value )}
          margin='dense'
          id='description'
          label='description'
          type='text'
          fullWidth/>
        <IssueTypeSelector initialValue={undefined} handleChange={handleIssueTypeChange}/>
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
