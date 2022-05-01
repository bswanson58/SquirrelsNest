import {FormControl, InputLabel, MenuItem, Select, SelectChangeEvent} from '@mui/material'
import {PropsWithChildren, useEffect, useState} from 'react'
import {selectProjectIssueTypes} from '../store/projects'
import {useAppSelector} from '../store/storeHooks'

interface IssueTypeSelectorProps {
  handleChange: ( event: SelectChangeEvent ) => void
}

export function IssueTypeSelector( props: PropsWithChildren<IssueTypeSelectorProps> ) {
  const [issueType, setIssueType] = useState( '' )
  const { handleChange } = props
  const issueTypes = useAppSelector( selectProjectIssueTypes )

  function localHandleChange( event: SelectChangeEvent ) {
    setIssueType( event.target.value )

    handleChange( event )
  }

  useEffect( () => {
    setIssueType( issueTypes.length > 0 ? issueTypes[0].id : '' )
  }, [issueTypes] )

  return (
    <FormControl sx={{ m: 1, minWidth: 220 }} size='small'>
      <InputLabel id='issue-type-select-label'>Issue Type</InputLabel>
      <Select
        labelId='issue-type-select-label'
        label='Issue Type'
        value={issueType}
        onChange={localHandleChange}
      >
        {issueTypes.map( ( it ) => (<MenuItem key={it.id} value={it.id}>{it.name}</MenuItem>) )}
      </Select>
    </FormControl>
  )
}
