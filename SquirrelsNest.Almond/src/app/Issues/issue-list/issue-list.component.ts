import {Component, OnDestroy, OnInit} from '@angular/core'
import {Store} from '@ngrx/store'
import {Observable, Subscription} from 'rxjs'
import {ClIssue} from '../../Data/graphQlTypes'
import {AppState} from '../../Store/app.reducer'
import {getSelectedProject} from '../../Store/app.selectors'
import {IssuesFacade} from '../issues.facade'
import {IssueService} from '../issues.service'

@Component( {
  selector: 'sn-issue-list',
  templateUrl: './issue-list.component.html',
  styleUrls: ['./issue-list.component.css']
} )
export class IssueListComponent implements OnInit, OnDestroy {
  issueList$: Observable<ClIssue[]>
  serverHasMoreIssues$: Observable<boolean>
  private mProjectSubscription: Subscription | undefined

  constructor( private store: Store<AppState>, private issuesService: IssueService, private issuesFacade: IssuesFacade ) {
    this.issueList$ = new Observable<ClIssue[]>()
    this.serverHasMoreIssues$ = new Observable<boolean>()
  }

  ngOnInit(): void {
    this.issueList$ = this.issuesFacade.GetCurrentIssuesList()
    this.serverHasMoreIssues$ = this.issuesFacade.GetServerHasMoreIssues()

    this.mProjectSubscription = this.store.select( getSelectedProject ).subscribe( project => {
      if( project != null ) {
        this.issuesFacade.LoadIssues()
      }
      else {
        this.issuesFacade.ClearIssues()
      }
    } )
  }

  onRetrieveMoreIssues() {
    this.issuesFacade.LoadMoreIssues()
  }

  ngOnDestroy() {
    this.mProjectSubscription?.unsubscribe()
    this.mProjectSubscription = undefined
  }
}
