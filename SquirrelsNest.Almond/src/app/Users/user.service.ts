import {Injectable} from '@angular/core'
import {GraphQLErrors} from '@apollo/client/errors'
import {Store} from '@ngrx/store'
import {Apollo, QueryRef} from 'apollo-angular'
import {map, Subscription, take, tap} from 'rxjs'
import {ClUserCollectionSegment, Query} from '../Data/graphQlTypes'
import {UserQuery, UserQueryInput} from '../Data/queryStatements'
import {AppState} from '../Store/app.reducer'
import {getUserQueryState} from '../Store/app.selectors'
import {AppendUsers, ClearUsers, ClearUsersLoading, SetUsersLoading} from './user.actions'
import {UserQueryInfo} from './user.state'

@Injectable( {
  providedIn: 'root'
} )
export class UserService {
  private readonly mPageLimit = 10
  private mUserQuery: QueryRef<Query, UserQueryInput> | null
  private mUsersSubscription: Subscription | null

  constructor( private apollo: Apollo, private store: Store<AppState> ) {
    this.mUsersSubscription = null
    this.mUserQuery = null
  }

  LoadUsers(): void {
    this.unsubscribe()
    this.store.dispatch( new SetUsersLoading() )
    this.store.dispatch( new ClearUsers() )

    this.mUserQuery = this.apollo.use( 'usersWatchClient' ).watchQuery<Query, UserQueryInput>(
      {
        query: UserQuery,
        variables: {
          skip: 0,
          take: this.mPageLimit,
          order: {
          },
        } as UserQueryInput
      } )

    this.mUsersSubscription = this.mUserQuery
      .valueChanges
      .pipe(
        map( result => UserService.handleQueryErrors( result.data, result.errors ) ),
        map( result => this.handleUserData( result ) ),
        tap( _ => this.store.dispatch( new ClearUsersLoading() ) )
      )
      .subscribe( { complete: () => console.log( 'LoadUsers completed.' ) } )
  }

  LoadMoreUsers(): void {
    const queryState = this.getUsersQueryState()

    if( (this.mUserQuery != null) &&
      (queryState.hasNextPage) ) {
      this.store.dispatch( new SetUsersLoading() )

      this.mUserQuery.fetchMore( {
        variables: {
          skip: queryState.loadedUsers
        }
      } ).then()
    }
  }

  private static handleQueryErrors( data: Query | null, errors: GraphQLErrors | undefined ): ClUserCollectionSegment {
    if( errors != null ) {
      console.log( errors.entries() )
    }

    if( (data?.userList?.items != null) &&
      (data.userList.pageInfo != null) ) {
      return data.userList
    }
    else {
      return { items: [], pageInfo: { hasNextPage: false, hasPreviousPage: false }, totalCount: 0 }
    }
  }

  private handleUserData( issueData: ClUserCollectionSegment ): void {
    this.store.dispatch(
      new AppendUsers( issueData.items!, {
        hasNextPage: issueData.pageInfo.hasNextPage,
        hasPreviousPage: issueData.pageInfo.hasPreviousPage,
        totalUsers: issueData.totalCount,
        loadedUsers: 0 // set by the reducer.
      } ) )
  }

  private unsubscribe() {
    if( this.mUsersSubscription != null ) {
      this.mUsersSubscription.unsubscribe()
      this.mUsersSubscription = null
    }
  }
  private getUsersQueryState(): UserQueryInfo {
    let queryState: UserQueryInfo

    this.store.select( getUserQueryState ).pipe( take( 1 ) ).subscribe( state => queryState = state )

    return queryState!
  }

  ngOnDestroy() {
    this.unsubscribe()
  }
}
