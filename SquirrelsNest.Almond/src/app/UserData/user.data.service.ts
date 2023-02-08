import {Injectable} from '@angular/core'
import {GraphQLErrors} from '@apollo/client/errors'
import {Store} from '@ngrx/store'
import {Apollo} from 'apollo-angular'
import {map, Observable} from 'rxjs'
import {GraphQlBaseService} from '../Common/graphql.base.service'
import {Mutation, Query, UserDataInput, UserDataPayload, UserDataType} from '../Data/graphQlTypes'
import {AppState} from '../Store/app.reducer'
import {UserData} from './user.data'
import {UserDataMutation, UserDataQuery} from './user.data.gql'

@Injectable( {
  providedIn: 'root'
} )
export class UserDataService extends GraphQlBaseService {

  constructor( store: Store<AppState>, private apollo: Apollo ) {
    super( store )
  }

  LoadUserData(): Observable<UserDataPayload> {
    const dataInput: UserDataInput = {
      dataType: 'ALMOND_CLIENT' as UserDataType,
      data: ''
    }

    return this.apollo.use( 'defaultClient' ).query<Query>( {
      fetchPolicy: 'no-cache',
      query: UserDataQuery,
      variables: { dataInput: dataInput }
    } )
      .pipe(
        map( result => UserDataService.handleQueryErrors( result.data, result.errors ) ),
        map( result => result as UserDataPayload )
      )
  }

  private static handleQueryErrors( data: Query | undefined | null, errors: GraphQLErrors | undefined ): UserDataPayload | null {
    if( errors != null ) {
      console.log( errors.entries() )
    }

    return data?.userData ?? null
  }

  SaveUserData( data: UserData ): void {
    const dataInput: UserDataInput = {
      dataType: 'ALMOND_CLIENT' as UserDataType,
      data: JSON.stringify( data )
    }

    this.apollo.use( 'defaultClient' ).mutate<Mutation>( {
      fetchPolicy: 'no-cache',
      mutation: UserDataMutation,
      variables: { dataInput: dataInput }
    } )
      .pipe(
        map( result => this.handleMutationErrors( result.data?.saveUserData, result.errors ) ),
        map( result => {
          const payload = result as UserDataPayload

          if( payload.userData == null ) {
            console.error( 'SaveUserData failed.' )
          }
        } ),
      )
      .subscribe( this.getServiceObserver() )
  }
}
