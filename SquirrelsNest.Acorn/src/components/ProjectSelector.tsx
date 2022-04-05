import { useEffect, useState } from 'react'
import Box from '@mui/material/Box'
import List from '@mui/material/List'
import ListItem from '@mui/material/ListItem'
import ListItemButton from '@mui/material/ListItemButton'
import ListItemText from '@mui/material/ListItemText'
import Typography from '@mui/material/Typography'
import { useProjectContext } from '../data/ProjectContext'
import { ClProject } from '../data/GraphQlEntities'

function ProjectSelector() {
  const projectData = useProjectContext()
  const [selectedItem, setSelectedItem] = useState<String>('');

  const handleListItemClick = (project: ClProject) => {
    projectData.setCurrentProject(project)
  };

  useEffect(() => {
    if(projectData.currentProject !== undefined) {
      setSelectedItem(projectData.currentProject.id)
    }
  },[projectData.currentProject])

  if(projectData.loadingErrors) {
    return <Box>An error occurred...</Box>
  }

  return (
    <>
      <Typography variant='subtitle2'>Projects</Typography>

      <List dense>
        {projectData.projects.projects.map((item) => (
          <ListItem key={item.id as React.Key} disablePadding>
            <ListItemButton selected={selectedItem === item.id} onClick={() => handleListItemClick(item)}>
              <ListItemText primaryTypographyProps={{variant: 'body1'}} primary={item.name} />
            </ListItemButton>
          </ListItem>
        ))}
      </List>
    </>
  )
}

export default ProjectSelector
