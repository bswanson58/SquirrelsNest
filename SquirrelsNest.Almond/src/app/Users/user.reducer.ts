import {Action} from '@ngrx/store'
import {
  ADD_USER, AddUser,
  APPEND_USERS, AppendUsers,
  CLEAR_USERS,
  CLEAR_USERS_LOADING,
  SET_USERS_LOADING
} from './user.actions'
import {initialUserQueryInfo, initialUserState, UserState} from './user.state'

export function usersReducer( state: UserState = initialUserState, action: Action ): UserState {
  switch( action.type ) {
    case ADD_USER:
      const addPayload = action as AddUser

      return {
        ...state,
        users: [...state.users, addPayload.user]
      }

    case APPEND_USERS:
      const appendPayload = action as AppendUsers
      const newUserList = [...state.users, ...appendPayload.users]

      return {
        ...state,
        users: newUserList,
        queryInfo: { ...appendPayload.queryInfo, loadedUsers: newUserList.length }
      }

    case CLEAR_USERS:
      return {
        ...state,
        users: [],
        queryInfo: initialUserQueryInfo
      }

    case SET_USERS_LOADING:
      return {
        ...state,
        isLoading: true
      }

    case CLEAR_USERS_LOADING:
      return {
        ...state,
        isLoading: false
      }

    default: {
      return state
    }
  }
}
