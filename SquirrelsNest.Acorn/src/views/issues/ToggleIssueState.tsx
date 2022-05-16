import CheckIcon from '@mui/icons-material/Check'
import {IconButton} from '@mui/material'
import React, {PropsWithChildren} from 'react'
import {ClIssue, ClWorkflowState, UpdateIssueInput} from '../../data/graphQlTypes'
import {updateIssue} from '../../store/issueMutations'
import {selectCurrentProject} from '../../store/projects'
import {useAppDispatch, useAppSelector} from '../../store/storeHooks'

interface toggleIssueStateProps {
  issue: ClIssue
}

function ToggleIssueState( props: PropsWithChildren<toggleIssueStateProps> ) {
  const dispatch = useAppDispatch()
  const currentProject = useAppSelector( selectCurrentProject )
  const issue = props.issue

  const updateState = ( state: ClWorkflowState ) => {
    const updateInfo: UpdateIssueInput = {
      issueId: issue.id,
      operations: [{
        // @ts-ignore
        path: 'WORKFLOW_STATE_ID',
        value: state.id,
      }]
    }

    dispatch( updateIssue( updateInfo ) )
  }

  const findNextState = ( currentState: ClWorkflowState, projectStates: ClWorkflowState[] ): ClWorkflowState | undefined => {
    if( projectStates.length > 0 ) {
      const states = [...projectStates, ...projectStates]
      const index = states.findIndex( s => s.id === currentState.id )

      if( index !== -1 ) {
        return states[index + 1]
      }
    }

    return undefined
  }

  const handleToggleState = () => {
    if( currentProject !== null ) {
      const currentCategory = issue.workflowState.category
      const isInitial = currentCategory === 'INITIAL'
      const isCompleted = currentCategory === 'TERMINAL' || currentCategory === 'COMPLETED'
      const initialState = currentProject.workflowStates.find( s => s.category === 'INITIAL' )
      let completedState = currentProject.workflowStates.find( s => s.category === 'COMPLETED' )
      if( completedState === undefined ) {
        completedState = currentProject.workflowStates.find( s => s.category === 'TERMINAL' )
      }
      let newState: ClWorkflowState | undefined

//      console.log( `current state: ${issue.workflowState.name}` )
//      console.log(JSON.stringify(currentProject))
//      console.log( `currentCategory: ${currentCategory}` )
//      console.log( `initialState: ${JSON.stringify( initialState )}` )
//      console.log( `completedState: ${JSON.stringify( completedState )}` )
//      console.log( `initial: ${initialState!.name}` )
//      console.log( `completed: ${completedState!.name}` )
//      console.log( `currently: ${isInitial ? 'initial' : isCompleted ? 'completed' : 'unknown'}` )

      if( isInitial ) {
        newState = completedState
      }

      if( isCompleted ) {
        newState = initialState
      }

      if( issue.workflowState.id === 'default' ) {
        newState = initialState
      }

      if( completedState === undefined ) {
        newState = completedState
      }

      if( newState === undefined ) {
        newState = findNextState( issue.workflowState, currentProject.workflowStates )
      }

      if( newState !== undefined ) {
        updateState( newState )
      }
    }
  }

  return (
    <IconButton onClick={handleToggleState}>
      <CheckIcon/>
    </IconButton>
  )
}

export default ToggleIssueState
