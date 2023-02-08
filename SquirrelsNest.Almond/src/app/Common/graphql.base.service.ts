import {ApolloError, GraphQLErrors} from '@apollo/client/errors'
import {Store} from '@ngrx/store'
import {Observer} from 'rxjs'
import {AppState} from '../Store/app.reducer'
import {ReportError, ServiceCallEnded} from '../UI/ui.actions'

export class GraphQlBaseService {
  constructor( protected store: Store<AppState> ) {
  }

  protected handleMutationErrors( data: any | null | undefined, errors: GraphQLErrors | undefined ): any | null {
    if( Array.isArray( errors ) ) {
      if( errors.length > 0 ) {
        this.store.dispatch( new ReportError( errors[0].message ) )
      }
      else {
        this.store.dispatch( new ReportError( 'Unknown error occurred' ) )
      }

      return null
    }

    if( data == null ) {
      this.store.dispatch( new ReportError( 'No data returned from the server' ) )
    }

    if( Array.isArray( data?.errors ) ) {
      if( data.errors.length > 0 ) {
        this.store.dispatch( new ReportError( data.errors[0].message ) )
      }
    }

    return data
  }

  getServiceObserver() {
    return {
      next: () => {
      },
      error: err => {
        const e = err as ApolloError

        this.store.dispatch( new ServiceCallEnded() )
        this.store.dispatch( new ReportError( e.message ) )
      },
      complete: () => {
        this.store.dispatch( new ServiceCallEnded() )
      }

    } as Observer<any>
  }
}
