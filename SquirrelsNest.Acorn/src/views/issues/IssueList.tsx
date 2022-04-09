import React, {useState} from 'react'
import {Box, IconButton, List, ListItem, ListItemButton, ListItemText, Typography} from '@mui/material'
import AddIssueIcon from '@mui/icons-material/AddCircle'
import DetailIcon from '@mui/icons-material/List'
import AddIssueDialog from './AddIssueDialog'
import {createPrimary, createSecondary, eDisplayStyle, nextDisplayStyle} from './IssueList.Items'
import {RelativeBox, TopRightStack} from './IssueList.styles'
import {AddIssueInput, useIssueMutationContext, useIssueQueryContext, useProjectQueryContext} from '../../data'

function IssueList() {
  const [displayStyle, setDisplayStyle] = useState( eDisplayStyle.TITLE_DESCRIPTION )
  const [addIssue, setAddIssue] = useState( false )
  const projectContext = useProjectQueryContext()
  const currentIssues = useIssueQueryContext()
  const issueMutations = useIssueMutationContext()

  const emptyAddIssue: AddIssueInput = { title: '', description: '', projectId: '' }

  const toggleStyle = () => setDisplayStyle( nextDisplayStyle( displayStyle ) )

  const displayAddIssue = () => setAddIssue( true )
  const closeAddIssue = () => setAddIssue( false )
  const handleAddIssue = ( issue: AddIssueInput ) => {
    issueMutations.addIssue( issue )
    setAddIssue( false )
  }

  if( projectContext?.currentProject === undefined ) {
    return <Box>Select a project to display...</Box>
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
                primary={createPrimary( projectContext.currentProject?.issuePrefix!, item )}
                secondary={createSecondary( displayStyle, item )}
              />
            </ListItemButton>
          </ListItem>
        ) )}
      </List>

      <AddIssueDialog initialValues={ emptyAddIssue } onClose={() => closeAddIssue()}
                      onConfirm={issue => handleAddIssue( issue )} open={addIssue}/>
    </RelativeBox>
  )
}

export default IssueList
