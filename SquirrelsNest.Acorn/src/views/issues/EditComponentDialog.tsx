import {Dialog, DialogActions, DialogContent, DialogTitle, SelectChangeEvent,} from '@mui/material'
import Button from '@mui/material/Button'
import React, {PropsWithChildren, useState} from 'react'
import {ComponentSelector} from '../../components/ComponentSelector'
import {ClIssue, UpdateIssueInput} from '../../data/graphQlTypes'
import {updateIssue} from '../../store/issueMutations'
import {useAppDispatch} from '../../store/storeHooks'
import {hideModal} from '../../store/uiActions'

interface editComponentProps {
  payload: {
    modalProps: ClIssue
  }
}

function EditComponentDialog( props: PropsWithChildren<editComponentProps> ) {
  const dispatch = useAppDispatch()
  const issue = props.payload.modalProps
  const [component, setComponent] = useState<string>( issue.component.id )

  const handleCancel = () => {
    dispatch( hideModal() )
  }

  const handleConfirm = () => {
    if( (component !== undefined) &&
      (component !== issue.component.id) ) {
      const updateInfo: UpdateIssueInput = {
        issueId: issue.id,
        operations: [{
          // @ts-ignore
          path: 'COMPONENT_ID',
          value: component,
        }]
      }

      dispatch( updateIssue( updateInfo ) )
    }

    dispatch( hideModal() )
  }

  function handleComponentChange( event: SelectChangeEvent ): void {
    setComponent( event.target.value )
  }

  return (
    <Dialog open={true} onClose={() => handleCancel()} aria-labelledby='Edit Component'
            PaperProps={{ sx: { width: '300px', height: '200px' } }}>
      <DialogTitle>Edit Component</DialogTitle>
      <DialogContent>
        <ComponentSelector initialValue={issue.component.id} handleChange={handleComponentChange}/>
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

export default EditComponentDialog
