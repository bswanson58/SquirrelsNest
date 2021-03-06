import {Grid, Typography} from '@mui/material'
import React from 'react'
import {ClIssue} from '../../data/graphQlTypes'
import {eDisplayStyle} from '../../store/ui'
import {SubTypography, StrikeThruTypography, DimmedTypography, UncasedButton} from './IssueList.styles'

export const createPrimary = ( issuePrefix: String, issue: ClIssue ) => {
  return (
    <>
      {issue.isFinalized ?
        <>
          <SubTypography variant='body1' display='inline'>(</SubTypography>
          <StrikeThruTypography variant='body1'
                                display='inline'>{issuePrefix}-{issue.issueNumber}</StrikeThruTypography>
          <SubTypography variant='body1' display='inline'>) </SubTypography>
          <DimmedTypography variant='body1' display='inline'>{issue.title}</DimmedTypography>
        </> :
        <>
          <SubTypography variant='body1'
                         display='inline'>({issuePrefix}-{issue.issueNumber}) </SubTypography>
          <Typography variant='body1' display='inline'>{issue.title}</Typography>
        </>
      }
    </>
  )
}

export const createSubTypography = ( text: String ) => {
  return <SubTypography variant='body2'>{text}</SubTypography>
}

export const descriptionDetails = ( issue: ClIssue ) => {
  return createSubTypography( issue.description )
}

export const fullDetails = ( issue: ClIssue,
                             onClickComponent: ( issue: ClIssue ) => void,
                             onClickIssueType: ( issue: ClIssue ) => void,
                             onClickAssigned: ( issue: ClIssue ) => void,
                             onCLickWorkflow: ( issue: ClIssue ) => void ) => {
  return (
    <>
      <SubTypography variant='body2'>{issue.description}</SubTypography>
      <Grid container spacing={1} columns={14}>
        <Grid item xs={1}/>
        <Grid item xs={3}>
          <UncasedButton variant='text' size='small' color='inherit'
                         onClick={() => onClickIssueType( issue )}>{createSubTypography( issue.issueType.name )}</UncasedButton>
        </Grid>
        <Grid item xs={3}>
          <UncasedButton variant='text' size='small' color='inherit'
                         onClick={() => onCLickWorkflow( issue )}>{createSubTypography( issue.workflowState.name )}</UncasedButton>
        </Grid>
        <Grid item xs={3}>
          <UncasedButton variant='text' size='small' color='inherit'
                         onClick={() => onClickComponent( issue )}>{createSubTypography( issue.component.name )}</UncasedButton>
        </Grid>
        <Grid item xs={3}>
          <UncasedButton variant='text' size='small' color='inherit'
                         onClick={() => onClickAssigned( issue )}>{createSubTypography( issue.assignedTo.name )}</UncasedButton>
        </Grid>
        <Grid item xs={1}/>
      </Grid>
    </>
  )
}

export const createSecondary = ( displayStyle: eDisplayStyle, issue: ClIssue,
                                 onClickComponent: ( issue: ClIssue ) => void,
                                 onClickIssueType: ( issue: ClIssue ) => void,
                                 onClickAssigned: ( issue: ClIssue ) => void,
                                 onClickWorkflow: ( issue: ClIssue ) => void ) => {
  switch( displayStyle ) {
    case eDisplayStyle.FULL_DETAILS:
      return fullDetails( issue, onClickComponent, onClickIssueType, onClickAssigned, onClickWorkflow )

    case eDisplayStyle.TITLE_DESCRIPTION:
      return descriptionDetails( issue )

    case eDisplayStyle.TITLE_ONLY:
      return null
  }
}
