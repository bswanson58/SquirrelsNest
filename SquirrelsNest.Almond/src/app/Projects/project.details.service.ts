import {Injectable} from '@angular/core'
import {GraphQLErrors} from '@apollo/client/errors'
import {Store} from '@ngrx/store'
import {Apollo} from 'apollo-angular'
import {map, tap} from 'rxjs'
import {AddProjectDetailInput, ClProject, Mutation} from '../Data/graphQlTypes'
import {AddProjectDetailMutation} from '../Data/projectDetailMutations'
import {AppState} from '../Store/app.reducer'
import {AddProjectDetail, ClearProjectLoading, SetProjectLoading} from './projects.actions'

@Injectable( {
  providedIn: 'root'
} )
export class ProjectDetailsService {

  constructor( private apollo: Apollo, private store: Store<AppState> ) {
  }

  AddProjectDetail( detail: AddProjectDetailInput ) {
    this.store.dispatch( new SetProjectLoading() )

    this.apollo.use( 'defaultClient' ).mutate<Mutation>( {
      mutation: AddProjectDetailMutation,
      variables: { detailInput: detail }
    } )
      .pipe(
        map( result => ProjectDetailsService.handleAddDetailMutationErrors( result.data, result.errors ) ),
        map( result => {
          if( result !== null ) {
            this.store.dispatch( new AddProjectDetail( result ) )
          }

          return result
        } ),
        tap( _ => this.store.dispatch( new ClearProjectLoading() ) ),
      )
      .subscribe()
  }

  private static handleAddDetailMutationErrors( data: Mutation | undefined | null, errors: GraphQLErrors | undefined ): ClProject | null {
    if( errors != null ) {
      console.log( errors.entries() )
    }

    if( (data?.addProjectDetail?.errors !== undefined) &&
      (data.addProjectDetail.project !== undefined) ) {
      return data.addProjectDetail.project
    }

    return null
  }
}
