import {Component, OnDestroy, OnInit} from '@angular/core'
import {Store} from '@ngrx/store'
import {Observable, Subscription} from 'rxjs'
import {ClIssue} from '../../Data/graphQlTypes'
import {AppState} from '../../Store/app.reducer'
import {getIssues, getSelectedProject, getServerHasMoreIssues} from '../../Store/app.selectors'
import {ClearIssues} from '../issues.actions'
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

  constructor( private store: Store<AppState>, private issuesService: IssueService ) {
    this.issueList$ = new Observable<ClIssue[]>()
    this.serverHasMoreIssues$ = new Observable<boolean>()
  }

  ngOnInit(): void {
    this.issueList$ = this.store.select( getIssues )
    this.serverHasMoreIssues$ = this.store.select( getServerHasMoreIssues )

    this.mProjectSubscription = this.store.select( getSelectedProject ).subscribe( project => {
      if( project != null ) {
        this.issuesService.LoadIssues( project.id )
      }
      else {
        this.store.dispatch( new ClearIssues() )
      }
    } )
  }

  onRetrieveMoreIssues() {
    this.issuesService.LoadMoreIssues()
  }

  ngOnDestroy() {
    this.mProjectSubscription?.unsubscribe()
    this.mProjectSubscription = undefined
  }
}
