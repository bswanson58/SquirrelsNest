import {Grid, Typography} from '@mui/material'
import React from 'react'
import {ClIssue} from '../../data'
import {SubTypography, StrikeThruTypography, DimmedTypography} from './IssueList.styles'

export enum eDisplayStyle { TITLE_ONLY, TITLE_DESCRIPTION, FULL_DETAILS }

export const nextDisplayStyle = ( displayStyle: eDisplayStyle ) => {
  switch( displayStyle ) {
    case eDisplayStyle.FULL_DETAILS:
      return eDisplayStyle.TITLE_DESCRIPTION
    case eDisplayStyle.TITLE_DESCRIPTION:
      return eDisplayStyle.TITLE_ONLY
    case eDisplayStyle.TITLE_ONLY:
      return eDisplayStyle.FULL_DETAILS
  }
}

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

export const fullDetails = ( issue: ClIssue ) => {
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

export const createSecondary = ( displayStyle: eDisplayStyle, issue: ClIssue ) => {
  switch( displayStyle ) {
    case eDisplayStyle.FULL_DETAILS:
      return fullDetails( issue )

    case eDisplayStyle.TITLE_DESCRIPTION:
      return descriptionDetails( issue )

    case eDisplayStyle.TITLE_ONLY:
      return null
  }
}
