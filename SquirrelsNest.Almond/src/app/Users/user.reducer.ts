import {Action} from '@ngrx/store'
import {
  ADD_USER, AddUser,
  APPEND_USERS, AppendUsers,
  CLEAR_USERS,
  CLEAR_USERS_LOADING,
  DELETE_USER, DeleteUser,
  SET_USERS_LOADING,
  UPDATE_USER, UpdateUser
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

    case DELETE_USER:
      const deletePayload = action as DeleteUser

      return {
        ...state,
        users: state.users.filter( u => u.email !== deletePayload.email )
      }

    case UPDATE_USER:
      const updatePayload = action as UpdateUser

      return {
        ...state,
        users: state.users.map( u => u.id === updatePayload.user.id ? updatePayload.user : u )
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
