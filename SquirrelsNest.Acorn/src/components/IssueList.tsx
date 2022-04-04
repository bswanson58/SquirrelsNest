import { Typography } from '@mui/material'
import Box from '@mui/material/Box'
import List from '@mui/material/List'
import ListItem from '@mui/material/ListItem'
import ListItemButton from '@mui/material/ListItemButton'
import ListItemText from '@mui/material/ListItemText'
import { useIssueContext } from '../data/IssueContext'

function IssueList() {
  const currentIssues = useIssueContext()

  if(currentIssues.loadingErrors) {
    return <Box>An error occurred...</Box>
  }

  return (
    <>
    <Typography variant='subtitle2'>Issues</Typography>

      <List dense>
        {currentIssues.issueData.issues.map((item) => (
          <ListItem key={item.id as React.Key} disablePadding>
            <ListItemButton>
              <ListItemText primary={item.title} />
            </ListItemButton>
          </ListItem>
        ))}
      </List>
    </>
  )
}

export default IssueList
