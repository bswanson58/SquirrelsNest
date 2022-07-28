import {Component, Input, OnInit} from '@angular/core'
import {Store} from '@ngrx/store'
import {Observable} from 'rxjs'
import {ClIssue} from '../../Data/graphQlTypes'
import {AppState} from '../../Store/app.reducer'
import {getIssueDisplayStyle} from '../../Store/app.selectors'
import {eIssueDisplayStyle} from '../../UI/ui.state'

@Component( {
  selector: 'sn-issue-detail',
  templateUrl: './issue-detail.component.html',
  styleUrls: ['./issue-detail.component.css']
} )
export class IssueDetailComponent implements OnInit {
  @Input() issue!: ClIssue

  issueListStyle$: Observable<eIssueDisplayStyle>

  constructor( private store: Store<AppState> ) {
    this.issueListStyle$ = new Observable<eIssueDisplayStyle>()
  }

  ngOnInit(): void {
    this.issueListStyle$ = this.store.select( getIssueDisplayStyle )
  }
}
