import {FormControl, InputLabel, MenuItem, Select, SelectChangeEvent} from '@mui/material'
import {PropsWithChildren, useEffect, useState} from 'react'
import {selectProjectComponents} from '../store/projects'
import {useAppSelector} from '../store/storeHooks'

interface ComponentSelectorProps {
  initialValue: string | undefined,
  handleChange: ( event: SelectChangeEvent ) => void
}

export function ComponentSelector( props: PropsWithChildren<ComponentSelectorProps> ) {
  const components = useAppSelector( selectProjectComponents )
  const [component, setComponent] = useState( '' )
  const { handleChange } = props

  function localHandleChange( event: SelectChangeEvent ) {
    setComponent( event.target.value )

    handleChange( event )
  }

  useEffect( () => {
    setComponent( props.initialValue !== undefined ? props.initialValue : components.length > 0 ? components[0].id : '' )
  }, [components, props.initialValue] )

  return (
    <FormControl sx={{ m: 1, minWidth: 220 }} size='small'>
      <InputLabel id='component-select-label'>Component</InputLabel>
      <Select
        labelId='component-select-label'
        label='Component'
        value={component}
        onChange={localHandleChange}
      >
        {components.map( ( it ) => (<MenuItem key={it.id} value={it.id}>{it.name}</MenuItem>) )}
      </Select>
    </FormControl>
  )
}
