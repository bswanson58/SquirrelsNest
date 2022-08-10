import {Injectable} from '@angular/core'
import {Store} from '@ngrx/store'
import {ClIssue} from '../Data/graphQlTypes'
import {AppState} from '../Store/app.reducer'
import {ToggleIssueListStyle} from '../UI/ui.actions'
import {IssueService} from './issues.service'

@Injectable( {
  providedIn: 'root'
} )
export class IssuesFacade {
  constructor( private store: Store<AppState>, private service: IssueService ) {
  }

  AddIssue( issue: ClIssue ) {
    this.service.AddIssue( issue )
  }

  ToggleIssueListStyle() {
    this.store.dispatch( new ToggleIssueListStyle() )
  }
}
