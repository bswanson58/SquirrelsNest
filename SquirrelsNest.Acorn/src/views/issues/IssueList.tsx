import React from 'react'
import {Box, Button, Grid, IconButton, List, ListItem, ListItemText, Typography} from '@mui/material'
import AddIssueIcon from '@mui/icons-material/AddCircle'
import CheckIcon from '@mui/icons-material/Check'
import DetailIcon from '@mui/icons-material/List'
import DeleteIcon from '@mui/icons-material/Delete'
import {showAddIssueModal, showDeleteIssueConfirm, showEditIssueTypeModal} from '../../config/modalMap'
import {ClIssue} from '../../data/graphQlTypes'
import {requestAdditionalIssues} from '../../store/issueActions'
import {selectIssueList, selectMoreIssuesAvailable} from '../../store/issues'
import {selectCurrentProject} from '../../store/projects'
import {useAppDispatch, useAppSelector} from '../../store/storeHooks'
import {selectIssueListStyle, toggleIssueListStyle} from '../../store/ui'
import {createPrimary, createSecondary} from './IssueList.Items'
import {RelativeBox, TopRightStack} from './IssueList.styles'

function IssueList() {
  const currentProject = useAppSelector( selectCurrentProject )
  const issueList = useAppSelector( selectIssueList )
  const issueListStyle = useAppSelector( selectIssueListStyle )
  const moreIssuesAvailable = useAppSelector( selectMoreIssuesAvailable )
  const dispatch = useAppDispatch()

  const toggleStyle = () => dispatch( toggleIssueListStyle() )
  const handleAddIssue = () => dispatch( showAddIssueModal() )

  const handleDelete = ( issue: ClIssue ) => dispatch( showDeleteIssueConfirm( issue ) )

  const onClickIssueType = ( issue: ClIssue ) => dispatch( showEditIssueTypeModal( issue ) )

  const loadAdditionalIssues = () => dispatch( requestAdditionalIssues() )

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
        <IconButton onClick={handleAddIssue}>
          <AddIssueIcon/>
        </IconButton>
        <IconButton onClick={toggleStyle}>
          <DetailIcon/>
        </IconButton>
      </TopRightStack>

      <List dense>
        {issueList.map( ( item ) => (
          <ListItem key={item.id as React.Key} disablePadding>
            <Grid container direction='row' alignItems='center' spacing={1}>
              <Grid item xs='auto'>
                <IconButton><CheckIcon/></IconButton>
              </Grid>
              <Grid item xs>
                <ListItemText
                  disableTypography={true}
                  primary={createPrimary( currentProject?.issuePrefix!, item )}
                  secondary={createSecondary( issueListStyle, item, onClickIssueType )}
                />
              </Grid>
              <Grid item xs='auto'>
                <IconButton onClick={() => handleDelete( item )}><DeleteIcon/></IconButton>
              </Grid>
            </Grid>
          </ListItem>
        ) )}
      </List>

      {moreIssuesAvailable &&
          <Button onClick={loadAdditionalIssues}>Load More Issues</Button>
      }
    </RelativeBox>
  )
}

export default IssueList
