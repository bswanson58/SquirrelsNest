import {createSlice, PayloadAction} from '@reduxjs/toolkit'
import {ClIssue, ClIssueCollectionSegment} from '../data/graphQlTypes'
import {RootState} from './configureStore'

interface IssueState {
  list: ClIssue[]
  listState: {
    skip: number
    take: number
    totalCount: number
  }
  loading: boolean
  mutating: boolean
}

const initialState: IssueState = {
  list: [],
  listState: {
    skip: 0,
    take: 3,
    totalCount: 0,
  },
  loading: false,
  mutating: false,
}

const slice = createSlice( {
  name: 'issues',
  initialState: initialState,
  // actions => actionHandlers
  reducers: {
    issueListPrepare: ( issueState ) => {
      issueState.list = []
      issueState.listState.totalCount = 0
      issueState.listState.skip = 0
    },

    issueListRequested: ( issueState ) => {
      issueState.loading = true
      issueState.listState.skip = issueState.list.length
    },

    issueListReceived: ( issueState, action: PayloadAction<ClIssueCollectionSegment> ) => {
      issueState.list = [...issueState.list, ...action.payload.items!]
      issueState.listState.totalCount = action.payload.totalCount
      issueState.loading = false

      console.log(`issue list received: ${issueState.list.length}`)
    },

    issueListFailed: ( issueState, action: PayloadAction<string> ) => {
      issueState.loading = false

      console.log( `issue list failed: ${action.payload}` )
    },

    issueMutationStarted: ( issueState ) => {
      issueState.mutating = true
    },

    issueAdded: ( issueState, action: PayloadAction<ClIssue> ) => {
      issueState.list = [action.payload, ...issueState.list]

      issueState.mutating = false
    },

    issueUpdated: ( issueState, action: PayloadAction<ClIssue> ) => {
      issueState.list = issueState.list.map( i => i.id === action.payload.id ? action.payload : i )
    },

    issueMutationFailed: ( issueState ) => {
      issueState.mutating = false
    },
  }
} )

export function selectIssueList( state: RootState ) : ClIssue[] {
  return state.entities.issues.list
}

export function selectMoreIssuesAvailable( state: RootState ) : boolean {
  return state.entities.issues.listState.totalCount > state.entities.issues.list.length
}

export const {
  issueListPrepare,
  issueListRequested,
  issueListReceived,
  issueListFailed,
  issueMutationStarted,
  issueMutationFailed,
  issueAdded,
  issueUpdated,
} = slice.actions

export default slice.reducer

// Action Creators
/*
export function addIssue( issue: AddIssueInput ) {
  console.log(`addIssue`)
  return ({
    type: issueAddBegin.type,
    payload: { issue }
  })
}
*/
export function addClIssue( issue: ClIssue) {
  console.log(`addClIssue`)
  return({
    type: issueAdded.type,
    payload: issue
  })
}

/*
export const resolveIssue = id =>
  queryCallBegan( {
    url: url + '/' + id,
    onSuccess: issueResolved.type,
    onStart: '',
    onFail: ''
  } ),
)
*/
