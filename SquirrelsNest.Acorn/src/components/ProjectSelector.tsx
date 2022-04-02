import * as React from 'react'
import Box from '@mui/material/Box'
import List from '@mui/material/List'
import ListItem from '@mui/material/ListItem'
import ListItemButton from '@mui/material/ListItemButton'
import ListItemText from '@mui/material/ListItemText'
import Typography from '@mui/material/Typography'
import { useQuery } from 'graphql-hooks'
import { PROJECTS_QUERY } from '../data/GraphQlQueries'
import { AllProjectsQueryResult } from '../data/GraphQlEntities'

function ProjectSelector() {
  const { loading, error, data } = useQuery<AllProjectsQueryResult>(
    PROJECTS_QUERY,
    {
      variables: {
        limit: 10,
      },
    }
  )

  if (loading) {
    return <Box>Loading...</Box>
  }

  if (error) {
    console.log('---- error details:')
    console.log(error)
    return <Box>An error occurred...</Box>
  }

  return (
    <>
      <Typography variant='subtitle2'>Projects</Typography>

      <List dense>
        {data?.allProjects.nodes.map((item) => (
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
