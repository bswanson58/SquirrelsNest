import {FormControl, InputLabel, MenuItem, Select, SelectChangeEvent} from '@mui/material'
import {PropsWithChildren, useEffect, useState} from 'react'
import {selectProjectWorkflowStates} from '../store/projects'
import {useAppSelector} from '../store/storeHooks'

interface WorkflowSelectorProps {
  initialValue: string | undefined,
  handleChange: ( event: SelectChangeEvent ) => void
}

export function WorkflowSelector( props: PropsWithChildren<WorkflowSelectorProps> ) {
  const states = useAppSelector( selectProjectWorkflowStates )
  const [state, setState] = useState( '' )
  const { handleChange } = props

  function localHandleChange( event: SelectChangeEvent ) {
    setState( event.target.value )

    handleChange( event )
  }

  useEffect( () => {
    setState( props.initialValue !== undefined ? props.initialValue : states.length > 0 ? states[0].id : '' )
  }, [states, props.initialValue] )

  return (
    <FormControl sx={{ m: 1, minWidth: 220 }} size='small'>
      <InputLabel id='workflow-state-select-label'>Workflow State</InputLabel>
      <Select
        labelId='workflow-state-select-label'
        label='Workflow State'
        value={state}
        onChange={localHandleChange}
      >
        {states.map( ( s ) => (<MenuItem key={s.id} value={s.id}>{s.name}</MenuItem>) )}
      </Select>
    </FormControl>
  )
}
