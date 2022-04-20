import {createSlice, PayloadAction} from '@reduxjs/toolkit'
import {AllProjectsConnection, ClProject} from '../data/graphQlTypes'

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

    setCurrentProject: (projectState, action: PayloadAction<ClProject> ) => {
      projectState.currentProject = action.payload

      console.log(`set current project: ${projectState.currentProject?.name}`)
    }
  }
} )

export const {
  projectListRequested,
  projectListReceived,
  projectListFailed,
  setCurrentProject,
} = slice.actions

export default slice.reducer
