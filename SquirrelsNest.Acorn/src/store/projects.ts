import {createSlice, PayloadAction} from '@reduxjs/toolkit'
import {AllProjectsConnection, ClProject} from '../data/graphQlTypes'
import {RootState} from './configureStore'

interface ProjectState {
  list: ClProject[]
  currentProject: ClProject | null
  loading: boolean
}

const initialState: ProjectState = {
  list: [],
  currentProject: null,
  loading: false
}

const slice = createSlice( {
  name: 'projects',
  initialState: initialState,
  // actions => actionHandlers
  reducers: {
    projectListRequested: ( projectState ) => {
      projectState.loading = true
      projectState.list = []

      console.log( `project list begin` )
    },

    projectListReceived: ( projectState, action: PayloadAction<AllProjectsConnection> ) => {
      if( action.payload.nodes !== undefined ) {
        projectState.list = action.payload.nodes!
      }

      projectState.loading = false

      console.log( `project list received: ${action.payload.nodes?.length}` )
    },

    projectListFailed: ( projectState, action: PayloadAction<string> ) => {
      projectState.loading = false

      console.log( `project list failed: ${action.payload}` )
    },

    projectSetCurrent: (projectState, action: PayloadAction<ClProject> ) => {
      projectState.currentProject = action.payload

      console.log(`set current project: ${projectState.currentProject?.name}`)
    }
  }
} )

export function selectProjectList(state: RootState ) : ClProject[] {
  return state.entities.projects.list
}

export function selectCurrentProject(state: RootState ) : ClProject | null {
  return state.entities.projects.currentProject
}

export const {
  projectListRequested,
  projectListReceived,
  projectListFailed,
  projectSetCurrent,
} = slice.actions

export default slice.reducer
