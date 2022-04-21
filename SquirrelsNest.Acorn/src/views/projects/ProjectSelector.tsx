import List from '@mui/material/List'
import ListItem from '@mui/material/ListItem'
import ListItemButton from '@mui/material/ListItemButton'
import ListItemText from '@mui/material/ListItemText'
import Typography from '@mui/material/Typography'
import React from 'react'
import {ClProject} from '../../data/graphQlTypes'
import {setCurrentProject} from '../../store/projectActions'
import {selectCurrentProject, selectProjectList} from '../../store/projects'
import {useAppDispatch, useAppSelector} from '../../store/storeHooks'

function ProjectSelector() {
  const projectList = useAppSelector( selectProjectList )
  const currentProject = useAppSelector( selectCurrentProject )
  const dispatch = useAppDispatch()

  const handleListItemClick = ( project: ClProject ) => {
    dispatch( setCurrentProject( project ) )
  }

  return (
    <>
      <Typography variant='subtitle2'>Projects</Typography>

      <List dense>
        {projectList.map( ( item ) => (
          <ListItem key={item.id as React.Key} disablePadding>
            <ListItemButton selected={currentProject?.id === item.id} onClick={() => handleListItemClick( item )}>
              <ListItemText primaryTypographyProps={{ variant: 'body1' }} primary={item.name}/>
            </ListItemButton>
          </ListItem>
        ) )}
      </List>
    </>
  )
}

export default ProjectSelector
