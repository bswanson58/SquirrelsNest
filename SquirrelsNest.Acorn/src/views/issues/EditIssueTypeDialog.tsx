import {Dialog, DialogActions, DialogContent, DialogTitle, SelectChangeEvent,} from '@mui/material'
import Button from '@mui/material/Button'
import React, {PropsWithChildren, useState} from 'react'
import {IssueTypeSelector} from '../../components/IssueTypeSelector'
import {ClIssue, UpdateIssueInput} from '../../data/graphQlTypes'
import {updateIssue} from '../../store/issueMutations'
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
      const updateInfo: UpdateIssueInput = {
        issueId: issue.id,
        operations: [{
          // @ts-ignore
          path: 'ISSUE_TYPE_ID',
          value: issueType,
        }]
      }

      console.log( `update issue type to ${issueType}` )

      dispatch( updateIssue( updateInfo ) )
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
