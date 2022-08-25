import {Injectable} from '@angular/core'
import {GraphQLErrors} from '@apollo/client/errors'
import {Store} from '@ngrx/store'
import {Apollo} from 'apollo-angular'
import {map, tap} from 'rxjs'
import {AddUserInput, ClUser, Mutation} from '../Data/graphQlTypes'
import {AddUserMutation} from '../Data/userMutations'
import {AppState} from '../Store/app.reducer'
import {AddUser, ClearUsersLoading, SetUsersLoading} from './user.actions'

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
}
