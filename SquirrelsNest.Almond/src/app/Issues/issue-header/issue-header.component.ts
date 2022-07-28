import {Component, OnInit} from '@angular/core'
import {Store} from '@ngrx/store'
import {Observable} from 'rxjs'
import {ClProject} from '../../Data/graphQlTypes'
import {AppState} from '../../Store/app.reducer'
import {getSelectedProject} from '../../Store/app.selectors'

@Component( {
  selector: 'sn-issue-header',
  templateUrl: './issue-header.component.html',
  styleUrls: ['./issue-header.component.css']
} )
export class IssueHeaderComponent implements OnInit {
  currentProject$: Observable<ClProject | null>

  constructor( private store: Store<AppState> ) {
    this.currentProject$ = new Observable<ClProject>()
  }

  ngOnInit(): void {
    this.currentProject$ = this.store.select( getSelectedProject )
  }

  onToggleListStyle() {
  }

  onCreateNewIssue() {
  }
}
