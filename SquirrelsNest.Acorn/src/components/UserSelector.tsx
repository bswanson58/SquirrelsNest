import {FormControl, InputLabel, MenuItem, Select, SelectChangeEvent} from '@mui/material'
import {PropsWithChildren, useEffect, useState} from 'react'
import {selectProjectUsers} from '../store/projects'
import {useAppSelector} from '../store/storeHooks'

interface ComponentSelectorProps {
  initialValue: string | undefined,
  handleChange: ( event: SelectChangeEvent ) => void
}

export function UserSelector( props: PropsWithChildren<ComponentSelectorProps> ) {
  const users = useAppSelector( selectProjectUsers )
  const [user, setUser] = useState( '' )
  const { handleChange } = props

  function localHandleChange( event: SelectChangeEvent ) {
    setUser( event.target.value )

    handleChange( event )
  }

  useEffect( () => {
    setUser( props.initialValue !== undefined ? props.initialValue : users.length > 0 ? users[0].id : '' )
  }, [users, props.initialValue] )

  return (
    <FormControl sx={{ m: 1, minWidth: 220 }} size='small'>
      <InputLabel id='user-select-label'>User</InputLabel>
      <Select
        labelId='user-select-label'
        label='User'
        value={user}
        onChange={localHandleChange}
      >
        {users.map( ( u ) => (<MenuItem key={u.id} value={u.id}>{u.name}</MenuItem>) )}
      </Select>
    </FormControl>
  )
}
