import {createSlice, PayloadAction} from '@reduxjs/toolkit'
import {ClIssue} from '../data/graphQlTypes'

interface IssueState {
  list: ClIssue[]
  loading: boolean
}

const initialState: IssueState = {
  list: [],
  loading: false
}

const slice = createSlice( {
  name: 'issues',
  initialState: initialState,
  // actions => actionHandlers
  reducers: {
    issueListRequested: ( issueState ) => {
      issueState.loading = true

      console.log(`issue list requested`)
    },

    issueListReceived: ( issueState, action: PayloadAction<ClIssue[]> ) => {
      issueState.list = action.payload
      issueState.loading = false

      console.log(`issue list received: ${issueState.list.length}`)
    },

    issueListFailed: ( issueState, action: PayloadAction<string> ) => {
      issueState.loading = false

      console.log( `issue list failed: ${action.payload}` )
    },

    // command - event
    // addBug - bugAdded
    issueAdded: ( issueState, action: PayloadAction<ClIssue> ) => {
      console.log('issueAdded reducer')

      issueState.list.push( action.payload )
    },
  }
} )

export const {
  issueListRequested,
  issueListReceived,
  issueListFailed,
  issueAdded,
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
