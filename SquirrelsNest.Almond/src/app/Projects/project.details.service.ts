import {Injectable} from '@angular/core'
import {GraphQLErrors} from '@apollo/client/errors'
import {Store} from '@ngrx/store'
import {Apollo} from 'apollo-angular'
import {map, tap} from 'rxjs'
import {ProjectDetailInput, ClProject, Mutation} from '../Data/graphQlTypes'
import {
  AddProjectDetailMutation,
  DeleteProjectDetailMutation,
  UpdateProjectDetailMutation
} from '../Data/projectDetailMutations'
import {AppState} from '../Store/app.reducer'
import {ClearProjectLoading, SetProjectLoading, UpdateProjectDetail} from './projects.actions'

@Injectable( {
  providedIn: 'root'
} )
export class ProjectDetailsService {

  constructor( private apollo: Apollo, private store: Store<AppState> ) {
  }

  AddProjectDetail( detail: ProjectDetailInput ) {
    this.store.dispatch( new SetProjectLoading() )

    this.apollo.use( 'defaultClient' ).mutate<Mutation>( {
      mutation: AddProjectDetailMutation,
      variables: { detailInput: detail }
    } )
      .pipe(
        map( result => ProjectDetailsService.handleAddDetailMutationErrors( result.data, result.errors ) ),
        map( result => {
          if( result !== null ) {
            this.store.dispatch( new UpdateProjectDetail( result ) )
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

  UpdateProjectDetail( detail: ProjectDetailInput ) {
    this.store.dispatch( new SetProjectLoading() )

    this.apollo.use( 'defaultClient' ).mutate<Mutation>( {
      mutation: UpdateProjectDetailMutation,
      variables: { detailInput: detail }
    } )
      .pipe(
        map( result => ProjectDetailsService.handleUpdateDetailMutationErrors( result.data, result.errors ) ),
        map( result => {
          if( result !== null ) {
            this.store.dispatch( new UpdateProjectDetail( result ) )
          }

          return result
        } ),
        tap( _ => this.store.dispatch( new ClearProjectLoading() ) ),
      )
      .subscribe()
  }

  private static handleUpdateDetailMutationErrors( data: Mutation | undefined | null, errors: GraphQLErrors | undefined ): ClProject | null {
    if( errors != null ) {
      console.log( errors.entries() )
    }

    if( (data?.updateProjectDetail?.errors !== undefined) &&
      (data.updateProjectDetail.project !== undefined) ) {
      return data.updateProjectDetail.project
    }

    return null
  }

  DeleteProjectDetail( detail: ProjectDetailInput ) {
    this.store.dispatch( new SetProjectLoading() )

    this.apollo.use( 'defaultClient' ).mutate<Mutation>( {
      mutation: DeleteProjectDetailMutation,
      variables: { detailInput: detail }
    } )
      .pipe(
        map( result => ProjectDetailsService.handleDeleteDetailMutationErrors( result.data, result.errors ) ),
        map( result => {
          if( result !== null ) {
            this.store.dispatch( new UpdateProjectDetail( result ) )
          }

          return result
        } ),
        tap( _ => this.store.dispatch( new ClearProjectLoading() ) ),
      )
      .subscribe()
  }

  private static handleDeleteDetailMutationErrors( data: Mutation | undefined | null, errors: GraphQLErrors | undefined ): ClProject | null {
    if( errors != null ) {
      console.log( errors.entries() )
    }

    if( (data?.deleteProjectDetail?.errors !== undefined) &&
      (data.deleteProjectDetail.project !== undefined) ) {
      return data.deleteProjectDetail.project
    }

    return null
  }

}
