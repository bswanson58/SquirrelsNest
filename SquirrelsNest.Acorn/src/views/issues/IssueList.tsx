import React, {useState} from 'react'
import {Box, Button, IconButton, List, ListItem, ListItemButton, ListItemText, Typography} from '@mui/material'
import AddIssueIcon from '@mui/icons-material/AddCircle'
import DetailIcon from '@mui/icons-material/List'
import {requestAdditionalIssues} from '../../store/issueActions'
import {selectIssueList} from '../../store/issues'
import {selectCurrentProject} from '../../store/projects'
import {useAppDispatch, useAppSelector} from '../../store/storeHooks'
import AddIssueDialog from './AddIssueDialog'
import {createPrimary, createSecondary, eDisplayStyle, nextDisplayStyle} from './IssueList.Items'
import {RelativeBox, TopRightStack} from './IssueList.styles'
import {AddIssueInput, useIssueMutationContext} from '../../data'

function IssueList() {
  const currentProject = useAppSelector(selectCurrentProject)
  const issueList = useAppSelector( selectIssueList )
  const totalIssueCount = 7;
  const dispatch = useAppDispatch()

  const [displayStyle, setDisplayStyle] = useState( eDisplayStyle.TITLE_DESCRIPTION )
  const [addIssue, setAddIssue] = useState( false )
  const issueMutations = useIssueMutationContext()

  const emptyAddIssue: AddIssueInput = { title: '', description: '', projectId: '' }

  const toggleStyle = () => setDisplayStyle( nextDisplayStyle( displayStyle ) )

  const loadAdditionalIssues = () => dispatch(requestAdditionalIssues)

  const displayAddIssue = () => setAddIssue( true )
  const closeAddIssue = () => setAddIssue( false )
  const handleAddIssue = ( issue: AddIssueInput ) => {
    issueMutations.addIssue( issue )
    setAddIssue( false )
  }

  if( currentProject === undefined ) {
    return <Box>Select a project to display...</Box>
  }

//  if( issueContext.loadingErrors ) {
//    return <Box>An error occurred...</Box>
//  }

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
        {issueList.map( ( item ) => (
          <ListItem key={item.id as React.Key} disablePadding>
            <ListItemButton>
              <ListItemText
                disableTypography={true}
                primary={createPrimary( currentProject?.issuePrefix!, item )}
                secondary={createSecondary( displayStyle, item )}
              />
            </ListItemButton>
          </ListItem>
        ) )}
      </List>

      {(issueList.length < totalIssueCount) &&
          <Button onClick={loadAdditionalIssues}>Load More Issues</Button>
      }

      <AddIssueDialog initialValues={emptyAddIssue} onClose={() => closeAddIssue()}
                      onConfirm={issue => handleAddIssue( issue )} open={addIssue}/>
    </RelativeBox>
  )
}

export default IssueList
