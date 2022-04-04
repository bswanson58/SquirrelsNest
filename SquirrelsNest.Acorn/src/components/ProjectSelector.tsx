import * as React from 'react'
import Box from '@mui/material/Box'
import List from '@mui/material/List'
import ListItem from '@mui/material/ListItem'
import ListItemButton from '@mui/material/ListItemButton'
import ListItemText from '@mui/material/ListItemText'
import Typography from '@mui/material/Typography'
import { useProjectContext } from '../data/ProjectContext'

function ProjectSelector() {
  const projectContext = useProjectContext()

  if(projectContext.loadingErrors) {
    return <Box>An error occurred...</Box>
  }

  return (
    <>
      <Typography variant='subtitle2'>Projects</Typography>

      <List dense>
        {projectContext.projects.projects.map((item) => (
          <ListItem key={item.id as React.Key} disablePadding>
            <ListItemButton>
              <ListItemText primary={item.name} />
            </ListItemButton>
          </ListItem>
        ))}
      </List>
    </>
  )
}

export default ProjectSelector
