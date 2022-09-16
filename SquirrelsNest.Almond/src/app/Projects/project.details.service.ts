import {Injectable} from '@angular/core'
import {Store} from '@ngrx/store'
import {Apollo} from 'apollo-angular'
import {map, tap} from 'rxjs'
import {GraphQlBaseService} from '../Common/graphql.base.service'
import {ProjectDetailInput, Mutation, ProjectDetailPayload} from '../Data/graphQlTypes'
import {
  AddProjectDetailMutation,
  DeleteProjectDetailMutation,
  UpdateProjectDetailMutation
} from '../Data/projectDetailMutations'
import {AppState} from '../Store/app.reducer'
import {ServiceCallEnded, ServiceCallStarted} from '../UI/ui.actions'
import {UpdateProjectDetail} from './projects.actions'

@Injectable( {
  providedIn: 'root'
} )
export class ProjectDetailsService extends GraphQlBaseService {

  constructor( private apollo: Apollo, store: Store<AppState> ) {
    super( store )
  }

  AddProjectDetail( detail: ProjectDetailInput ) {
    this.store.dispatch( new ServiceCallStarted( 'Adding Project Detail' ) )

    this.apollo.use( 'defaultClient' ).mutate<Mutation>( {
      mutation: AddProjectDetailMutation,
      variables: { detailInput: detail }
    } )
      .pipe(
        map( result => this.handleMutationErrors( result.data?.addProjectDetail, result.errors ) ),
        map( result => {
          const payload = result as ProjectDetailPayload

          if( payload.project != null ) {
            this.store.dispatch( new UpdateProjectDetail( payload.project ) )
          }

          return payload.project
        } ),
        tap( _ => this.store.dispatch( new ServiceCallEnded() ) ),
      )
      .subscribe()
  }

  UpdateProjectDetail( detail: ProjectDetailInput ) {
    this.store.dispatch( new ServiceCallStarted('Updating Project Detail') )

    this.apollo.use( 'defaultClient' ).mutate<Mutation>( {
      mutation: UpdateProjectDetailMutation,
      variables: { detailInput: detail }
    } )
      .pipe(
        map( result => this.handleMutationErrors( result.data?.updateProjectDetail, result.errors ) ),
        map( result => {
          const payload = result as ProjectDetailPayload

          if( payload.project != null ) {
            this.store.dispatch( new UpdateProjectDetail( payload.project ) )
          }

          return payload.project
        } ),
        tap( _ => this.store.dispatch( new ServiceCallEnded() ) ),
      )
      .subscribe()
  }

  DeleteProjectDetail( detail: ProjectDetailInput ) {
    this.store.dispatch( new ServiceCallStarted('Deleting Project Detail') )

    this.apollo.use( 'defaultClient' ).mutate<Mutation>( {
      mutation: DeleteProjectDetailMutation,
      variables: { detailInput: detail }
    } )
      .pipe(
        map( result => this.handleMutationErrors( result.data?.deleteProjectDetail, result.errors ) ),
        map( result => {
          const payload = result as ProjectDetailPayload

          if( payload.project != null ) {
            this.store.dispatch( new UpdateProjectDetail( payload.project ) )
          }

          return payload.project
        } ),
        tap( _ => this.store.dispatch( new ServiceCallEnded() ) ),
      )
      .subscribe()
  }
}
