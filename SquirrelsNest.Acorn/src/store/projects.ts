import {createSlice, PayloadAction} from '@reduxjs/toolkit'
import {ClComponent, ClIssueType, ClProject, ClProjectCollectionSegment, ClUser} from '../data/graphQlTypes'
import {RootState} from './configureStore'

interface ProjectState {
  list: ClProject[]
  listState: {
    skip: number
    take: number
    totalCount: number
  }
  currentProject: ClProject | null
  loading: boolean
}

const initialState: ProjectState = {
  list: [],
  listState: {
    skip: 0,
    take: 10,
    totalCount: 0,
  },
  currentProject: null,
  loading: false
}

const slice = createSlice( {
  name: 'projects',
  initialState: initialState,
  // actions => actionHandlers
  reducers: {
    projectListPrepare: ( projectState ) => {
      projectState.list = []
      projectState.listState.totalCount = 0
      projectState.listState.skip = 0
    },

    projectListRequested: ( projectState ) => {
      projectState.loading = true
      projectState.listState.skip = projectState.list.length
    },

    projectListReceived: ( projectState, action: PayloadAction<ClProjectCollectionSegment> ) => {
      projectState.list = [...projectState.list, ...action.payload.items!]
      projectState.listState.totalCount = action.payload.totalCount
      projectState.loading = false

      console.log( `project list received: ${action.payload.items?.length}` )
    },

    projectListFailed: ( projectState, action: PayloadAction<string> ) => {
      projectState.loading = false

      console.log( `project list failed: ${action.payload}` )
    },

    projectSetCurrent: ( projectState, action: PayloadAction<ClProject> ) => {
      projectState.currentProject = action.payload

      console.log( `set current project: ${projectState.currentProject?.name}` )
    }
  }
} )

export function selectProjectList( state: RootState ): ClProject[] {
  return state.entities.projects.list
}

export function selectCurrentProject( state: RootState ): ClProject | null {
  return state.entities.projects.currentProject
}

export function selectProjectIssueTypes( state: RootState ): ClIssueType[] {
  const project = state.entities.projects.currentProject

  if( project !== null ) {
    return project.issueTypes
  }

  return []
}

export function selectProjectComponents( state: RootState ): ClComponent[] {
  const project = state.entities.projects.currentProject

  if( project !== null ) {
    return project.components
  }

  return []
}

export function selectProjectUsers( state: RootState ): ClUser[] {
  const project = state.entities.projects.currentProject

  if( project !== null ) {
    return project.users
  }

  return []
}
export const {
  projectListPrepare,
  projectListRequested,
  projectListReceived,
  projectListFailed,
  projectSetCurrent,
} = slice.actions

export default slice.reducer
