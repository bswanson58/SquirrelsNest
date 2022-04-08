import Box from '@mui/material/Box'
import IconButton from '@mui/material/IconButton'
import AddIssue from '@mui/icons-material/AddCircle'
import List from '@mui/material/List'
import ListItem from '@mui/material/ListItem'
import ListItemButton from '@mui/material/ListItemButton'
import ListItemText from '@mui/material/ListItemText'
import Typography from '@mui/material/Typography'
import {UseClientRequestResult, useManualQuery} from 'graphql-hooks'
import {ClIssue} from '../data/GraphQlEntities'
import {ADD_ISSUE_MUTATION} from '../data/graphQlMutations'
import {useIssueQueryContext} from '../data/IssueQueryContext'
import styled from 'styled-components'
import {Grid, Stack} from '@mui/material'
import React, {useEffect, useState} from 'react'
import DetailIcon from '@mui/icons-material/List'
import {useProjectContext} from '../data/ProjectContext'
import AddIssueDialog from '../dialogs/AddIssueDialog'
import {AddIssueInput, AddIssuePayload} from '../data/mutationEntities'

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
  const [issueInput, setIssueInput] = useState<AddIssueInput>({ title:'', description:'', projectId:''})
  const [addIssue, setAddIssue] = useState( false )
  const { currentProject } = useProjectContext()
  const currentIssues = useIssueQueryContext()

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

  const [requestAddIssue, mutationResult] = useManualQuery<AddIssuePayload>(
    ADD_ISSUE_MUTATION,
    {
      variables: {
        'issue': {
          'projectId': issueInput.projectId,
          'title': issueInput.title,
          'description': issueInput.description
        }
      },
    }
  )

  const processData = ( data: AddIssuePayload) => {
    console.log(`Added issue: ${data.addIssue.issue.title}`)

    if(data.addIssue.errors.length > 0){
      console.log(`Error adding issue: ${data.addIssue.errors[0]}`)
    }
    else {
      currentIssues.updateIssue(data.addIssue.issue)
    }
  }

  const processResponse = ( queryResult: UseClientRequestResult<AddIssuePayload> ) => {
    const { loading, error, data } = queryResult

    if (loading) {
      return
    }

    if (error) {
      console.log(error)

      return
    }

    if (data) {
      processData(data)
    }
  }

  useEffect(() => {
    if(issueInput?.projectId.length > 0){
      (async () => requestAddIssue())()
    }
  },[issueInput, requestAddIssue])

  useEffect(() => {
    processResponse(mutationResult)
  }, [mutationResult])

  const displayAddIssue = () => setAddIssue( true )
  const closeAddIssue = () => setAddIssue( false )
  const handleAddIssue = ( issue: AddIssueInput ) => {
    setAddIssue( false )
    setIssueInput(issue)
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
          <AddIssue/>
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
