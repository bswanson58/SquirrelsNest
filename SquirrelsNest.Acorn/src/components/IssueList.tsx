import Box from '@mui/material/Box'
import List from '@mui/material/List'
import ListItem from '@mui/material/ListItem'
import ListItemButton from '@mui/material/ListItemButton'
import ListItemText from '@mui/material/ListItemText'
import Typography from '@mui/material/Typography'
import { useIssueContext } from '../data/IssueContext'
import { ClIssue } from '../data/GraphQlEntities'
import styled from 'styled-components'

function IssueList() {
  const currentIssues = useIssueContext()

  const SubTypography = styled(Typography)`
    opacity: 0.7;
//    transform: scale(1);
//    -webkit-transform-origin-x: 0; // align text left after scaling
  `

  const createPrimary = (issue: ClIssue) => {
    return <Typography variant='body1'>{issue.title}</Typography>
  }

  const createSecondary = (issue: ClIssue) => {
    return <SubTypography variant='body2'>{issue.description}</SubTypography>
  }

  if (currentIssues.loadingErrors) {
    return <Box>An error occurred...</Box>
  }

  return (
    <>
      <Typography variant='subtitle2'>Issues</Typography>

      <List dense>
        {currentIssues.issueData.issues.map((item) => (
          <ListItem key={item.id as React.Key} disablePadding>
            <ListItemButton>
              <ListItemText
                disableTypography={true}
                primary={createPrimary(item)}
                secondary={createSecondary(item)}
              />
            </ListItemButton>
          </ListItem>
        ))}
      </List>
    </>
  )
}

export default IssueList
