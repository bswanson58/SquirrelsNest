import Box from '@mui/material/Box'
import IconButton from '@mui/material/IconButton'
import List from '@mui/material/List'
import ListItem from '@mui/material/ListItem'
import ListItemButton from '@mui/material/ListItemButton'
import ListItemText from '@mui/material/ListItemText'
import Typography from '@mui/material/Typography'
import { useIssueContext } from '../data/IssueContext'
import { ClIssue } from '../data/GraphQlEntities'
import styled from 'styled-components'
import { Grid } from '@mui/material'
import { useState } from 'react'
import DetailIcon from '@mui/icons-material/List';

const RelativeBox = styled(Box)`
  position: relative;
`
const TopRightButton = styled(IconButton)`
  position: absolute;
  right: 0%;
  top: 0%;
  padding: 0%;
  margin: 0%;
`
const SubTypography = styled(Typography)`
  opacity: 0.7;
  // transform: scale(1);
  // -webkit-transform-origin-x: 0; // align text left after scaling
  `
enum eDisplayStyle { TITLE_ONLY, TITLE_DESCRIPTION, FULL_DETAILS }

function IssueList() {
  const [displayStyle, setDisplayStyle] = useState(eDisplayStyle.TITLE_DESCRIPTION)
  const currentIssues = useIssueContext()

  const toggleStyle = () => {
    switch(displayStyle) {
      case eDisplayStyle.FULL_DETAILS:
        setDisplayStyle(eDisplayStyle.TITLE_DESCRIPTION)
        break
      case eDisplayStyle.TITLE_DESCRIPTION:
        setDisplayStyle(eDisplayStyle.TITLE_ONLY)
        break
      case eDisplayStyle.TITLE_ONLY:
        setDisplayStyle(eDisplayStyle.FULL_DETAILS)
        break
    }
  }
  
  const createPrimary = (issue: ClIssue) => {
    return <Typography variant='body1'>{issue.title}</Typography>
  }

  const createSubTypography = (text: String) => {
    return <SubTypography variant='body2'>{text}</SubTypography>
  }

  const descriptionDetails = (issue: ClIssue) => {
    return createSubTypography(issue.description)
  }

  const fullDetails = (issue: ClIssue) => {
    return(
      <>
        <SubTypography variant='body2'>{issue.description}</SubTypography>
        <Grid container spacing={1} columns={14}>
          <Grid item xs={1}/>
          <Grid item xs={3}>{createSubTypography(issue.issueType.name)}</Grid>
          <Grid item xs={3}>{createSubTypography(issue.workflowState.name)}</Grid>
          <Grid item xs={3}>{createSubTypography(issue.component.name)}</Grid>
          <Grid item xs={3}>{createSubTypography(issue.assignedTo.name)}</Grid>
          <Grid item xs={1}/>
        </Grid>
      </>
    )
  }

  const createSecondary = (issue: ClIssue) => {
    switch(displayStyle) {
      case eDisplayStyle.FULL_DETAILS:
        return fullDetails(issue)

      case eDisplayStyle.TITLE_DESCRIPTION:
        return descriptionDetails(issue)

      case eDisplayStyle.TITLE_ONLY:
        return(null)
    }
  }

  if (currentIssues.loadingErrors) {
    return <Box>An error occurred...</Box>
  }

  return (
    <RelativeBox>
      <Typography variant='subtitle2'>Issues</Typography>
      <TopRightButton onClick={toggleStyle}>
        <DetailIcon/>
      </TopRightButton>

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
    </RelativeBox>
  )
}

export default IssueList
