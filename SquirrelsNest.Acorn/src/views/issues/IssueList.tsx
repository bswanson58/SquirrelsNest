import React, {useState} from 'react'
import {Box, Button, IconButton, List, ListItem, ListItemButton, ListItemText, Typography} from '@mui/material'
import AddIssueIcon from '@mui/icons-material/AddCircle'
import DetailIcon from '@mui/icons-material/List'
import {requestAdditionalIssues} from '../../store/issueActions'
import {selectIssueList, selectMoreIssuesAvailable} from '../../store/issues'
import {selectCurrentProject} from '../../store/projects'
import {useAppDispatch, useAppSelector} from '../../store/storeHooks'
import {selectIssueListStyle, toggleIssueListStyle} from '../../store/ui'
import AddIssueDialog from './AddIssueDialog'
import {createPrimary, createSecondary} from './IssueList.Items'
import {RelativeBox, TopRightStack} from './IssueList.styles'
import {AddIssueInput} from '../../data/graphQlTypes'
import {addIssue} from '../../store/issueActions'

function IssueList() {
  const currentProject = useAppSelector( selectCurrentProject )
  const issueList = useAppSelector( selectIssueList )
  const issueListStyle = useAppSelector( selectIssueListStyle )
  const moreIssuesAvailable = useAppSelector( selectMoreIssuesAvailable )
  const dispatch = useAppDispatch()

  const [displayIssueDialog, setDisplayIssueDialog] = useState( false )

  const emptyAddIssue: AddIssueInput = { title: '', description: '', projectId: '' }

  const loadAdditionalIssues = () => dispatch( requestAdditionalIssues() )
  const toggleStyle = () => dispatch( toggleIssueListStyle() )

  const displayAddIssue = () => setDisplayIssueDialog( true )
  const closeAddIssue = () => setDisplayIssueDialog( false )
  const handleAddIssue = ( issue: AddIssueInput ) => {
    dispatch( addIssue( issue ) )
    setDisplayIssueDialog( false )
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
                secondary={createSecondary( issueListStyle, item )}
              />
            </ListItemButton>
          </ListItem>
        ) )}
      </List>

      {moreIssuesAvailable &&
          <Button onClick={loadAdditionalIssues}>Load More Issues</Button>
      }

      <AddIssueDialog initialValues={emptyAddIssue} onClose={() => closeAddIssue()}
                      onConfirm={issue => handleAddIssue( issue )} open={displayIssueDialog}/>
    </RelativeBox>
  )
}

export default IssueList
