import React, {useState} from 'react'
import {Box, Grid, IconButton, List, ListItem, ListItemButton, ListItemText, Stack, Typography} from '@mui/material'
import AddIssueIcon from '@mui/icons-material/AddCircle'
import DetailIcon from '@mui/icons-material/List'
import styled from 'styled-components'
import AddIssueDialog from './AddIssueDialog'
import {AddIssueInput, ClIssue, useIssueMutationContext, useIssueQueryContext, useProjectQueryContext} from '../../data'

const RelativeBox = styled( Box )`
  position: relative;
`
const TopRightStack = styled( Stack )`
  position: absolute;
  right: 0;
  top: 0;
  padding: 0;
  margin: 0;
`
const SubTypography = styled( Typography )`
  opacity: 0.7;
  // transform: scale(1);
  // -webkit-transform-origin-x: 0; // align text left after scaling
`

enum eDisplayStyle { TITLE_ONLY, TITLE_DESCRIPTION, FULL_DETAILS }

function IssueList() {
  const [displayStyle, setDisplayStyle] = useState( eDisplayStyle.TITLE_DESCRIPTION )
  const [addIssue, setAddIssue] = useState( false )
  const { currentProject } = useProjectQueryContext()
  const currentIssues = useIssueQueryContext()
  const issueMutations = useIssueMutationContext()

  const toggleStyle = () => {
    switch( displayStyle ) {
      case eDisplayStyle.FULL_DETAILS:
        setDisplayStyle( eDisplayStyle.TITLE_DESCRIPTION )
        break
      case eDisplayStyle.TITLE_DESCRIPTION:
        setDisplayStyle( eDisplayStyle.TITLE_ONLY )
        break
      case eDisplayStyle.TITLE_ONLY:
        setDisplayStyle( eDisplayStyle.FULL_DETAILS )
        break
    }
  }

  const displayAddIssue = () => setAddIssue( true )
  const closeAddIssue = () => setAddIssue( false )
  const handleAddIssue = ( issue: AddIssueInput ) => {
    issueMutations.addIssue( issue )
    setAddIssue( false )
  }

  const createPrimary = ( issue: ClIssue ) => {
    return (
      <>
        <SubTypography variant='body1'
                       display='inline'>({currentProject?.issuePrefix}-{issue.issueNumber}) </SubTypography>
        <Typography variant='body1' display='inline'>{issue.title}</Typography>
      </>
    )
  }

  const createSubTypography = ( text: String ) => {
    return <SubTypography variant='body2'>{text}</SubTypography>
  }

  const descriptionDetails = ( issue: ClIssue ) => {
    return createSubTypography( issue.description )
  }

  const fullDetails = ( issue: ClIssue ) => {
    return (
      <>
        <SubTypography variant='body2'>{issue.description}</SubTypography>
        <Grid container spacing={1} columns={14}>
          <Grid item xs={1}/>
          <Grid item xs={3}>{createSubTypography( issue.issueType.name )}</Grid>
          <Grid item xs={3}>{createSubTypography( issue.workflowState.name )}</Grid>
          <Grid item xs={3}>{createSubTypography( issue.component.name )}</Grid>
          <Grid item xs={3}>{createSubTypography( issue.assignedTo.name )}</Grid>
          <Grid item xs={1}/>
        </Grid>
      </>
    )
  }

  const createSecondary = ( issue: ClIssue ) => {
    switch( displayStyle ) {
      case eDisplayStyle.FULL_DETAILS:
        return fullDetails( issue )

      case eDisplayStyle.TITLE_DESCRIPTION:
        return descriptionDetails( issue )

      case eDisplayStyle.TITLE_ONLY:
        return null
    }
  }

  if( currentIssues.loadingErrors ) {
    return <Box>An error occurred...</Box>
  }

  return (
    <RelativeBox>
      <Typography variant='subtitle2'>Issues</Typography>

      <TopRightStack direction='row'>
        <IconButton onClick={displayAddIssue}>
          <AddIssueIcon/>
        </IconButton>
        <IconButton onClick={toggleStyle}>
          <DetailIcon/>
        </IconButton>
      </TopRightStack>

      <List dense>
        {currentIssues.issueData.issues.map( ( item ) => (
          <ListItem key={item.id as React.Key} disablePadding>
            <ListItemButton>
              <ListItemText
                disableTypography={true}
                primary={createPrimary( item )}
                secondary={createSecondary( item )}
              />
            </ListItemButton>
          </ListItem>
        ) )}
      </List>

      <AddIssueDialog initialValues={{ title: '', description: '', projectId: '' }} onClose={() => closeAddIssue()}
                      onConfirm={issue => handleAddIssue( issue )} open={addIssue}/>
    </RelativeBox>
  )
}

export default IssueList
