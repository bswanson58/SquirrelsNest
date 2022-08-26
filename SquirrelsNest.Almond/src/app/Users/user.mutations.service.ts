import {Injectable} from '@angular/core'
import {GraphQLErrors} from '@apollo/client/errors'
import {Store} from '@ngrx/store'
import {Apollo} from 'apollo-angular'
import {map, tap} from 'rxjs'
import {AddUserInput, ClUser, DeleteUserInput, EditUserRolesInput, Mutation} from '../Data/graphQlTypes'
import {AddUserMutation, DeleteUserMutation, EditUserRolesMutation} from '../Data/userMutations'
import {AppState} from '../Store/app.reducer'
import {AddUser, ClearUsersLoading, DeleteUser, SetUsersLoading, UpdateUser} from './user.actions'

@Injectable( {
  providedIn: 'root'
} )
export class UserMutationsService {
  constructor( private apollo: Apollo, private store: Store<AppState> ) {
  }

  AddUser( userInput: AddUserInput ) {
    this.store.dispatch( new SetUsersLoading() )

    this.apollo.use( 'defaultClient' ).mutate<Mutation>( {
      mutation: AddUserMutation,
      variables: { userInput: userInput }
    } )
      .pipe(
        map( result => UserMutationsService.handleAddMutationErrors( result.data, result.errors ) ),
        map( result => {
          if( result !== null ) {
            this.store.dispatch( new AddUser( result ) )
          }

          return result
        } ),
        tap( _ => this.store.dispatch( new ClearUsersLoading() ) ),
      )
      .subscribe()
  }

  private static handleAddMutationErrors( data: Mutation | undefined | null, errors: GraphQLErrors | undefined ): ClUser | null {
    if( errors != null ) {
      console.log( errors.entries() )
    }

    if( (data?.addUser?.errors !== undefined) &&
      (data.addUser.user !== undefined) ) {
      return data.addUser.user
    }

    return null
  }

  DeleteUser( user: ClUser ) {
    const deleteUser: DeleteUserInput = {
      email: user.email
    }

    this.store.dispatch( new SetUsersLoading() )

    this.apollo.use( 'defaultClient' ).mutate<Mutation>( {
      mutation: DeleteUserMutation,
      variables: { deleteInput: deleteUser }
    } )
      .pipe(
        map( result => UserMutationsService.handleDeleteMutationErrors( result.data, result.errors ) ),
        map( result => {
          if( result !== null ) {
            this.store.dispatch( new DeleteUser( result ) )
          }

          return result
        } ),
        tap( _ => this.store.dispatch( new ClearUsersLoading() ) ),
      )
      .subscribe()
  }

  private static handleDeleteMutationErrors( data: Mutation | undefined | null, errors: GraphQLErrors | undefined ): string | null {
    if( errors != null ) {
      console.log( errors.entries() )
    }

    if( (data?.deleteUser?.errors !== undefined) &&
      (data.deleteUser.email !== undefined) ) {
      return data.deleteUser.email
    }

    return null
  }

  UpdateUserRoles( user: ClUser ) {
    const rolesInput: EditUserRolesInput = {
      email: user.email,
      claims: user.claims
    }

    this.store.dispatch( new SetUsersLoading() )

    this.apollo.use( 'defaultClient' ).mutate<Mutation>( {
      mutation: EditUserRolesMutation,
      variables: { rolesInput: rolesInput }
    } )
      .pipe(
        map( result => UserMutationsService.handleUpdateRolesMutationErrors( result.data, result.errors ) ),
        map( result => {
          if( result !== null ) {
            this.store.dispatch( new UpdateUser( result ) )
          }

          return result
        } ),
        tap( _ => this.store.dispatch( new ClearUsersLoading() ) ),
      )
      .subscribe()
  }

  private static handleUpdateRolesMutationErrors( data: Mutation | undefined | null, errors: GraphQLErrors | undefined ): ClUser | null {
    if( errors != null ) {
      console.log( errors.entries() )
    }

    if( (data?.editUserRoles?.errors !== undefined) &&
      (data.editUserRoles.user !== undefined) ) {
      return data.editUserRoles.user
    }

    return null
  }
}
