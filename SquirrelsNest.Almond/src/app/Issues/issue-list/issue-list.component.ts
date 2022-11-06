import {Component, OnDestroy, OnInit} from '@angular/core'
import {combineLatest, map, Observable, Subscription} from 'rxjs'
import {AuthFacade} from '../../Auth/auth.facade'
import {ClIssue} from '../../Data/graphQlTypes'
import {ProjectFacade} from '../../Projects/project.facade'
import {UiFacade} from '../../UI/ui.facade'
import {IssuesFacade} from '../issues.facade'

@Component( {
  selector: 'sn-issue-list',
  templateUrl: './issue-list.component.html',
  styleUrls: ['./issue-list.component.css']
} )
export class IssueListComponent implements OnInit, OnDestroy {
  issueList$: Observable<ClIssue[]>
  userId$: Observable<string>

  serverHasMoreIssues$: Observable<boolean>
  private mProjectSubscription: Subscription | undefined

  constructor( private projectFacade: ProjectFacade, private authFacade: AuthFacade, private issuesFacade: IssuesFacade, private uiFacade: UiFacade ) {
    this.serverHasMoreIssues$ = this.issuesFacade.GetServerHasMoreIssues$()

    this.userId$ =
      this.authFacade.GetAuthenticationClaims$()
        .pipe(
          map( claims => {
            const idClaim = claims.find( c => c.name === 'entityId' )

            return idClaim ? idClaim.value : ''
          } ) )

    this.issueList$ =
      combineLatest( [
        this.issuesFacade.GetCurrentIssuesList$(),
        this.userId$,
        uiFacade.GetDisplayOnlyMyIssues$(),
        uiFacade.GetDisplayCompletedIssues$()] )
        .pipe(
          map( ( [list, userId, onlyMine, displayCompleted] ) => {
            return { list, userId, onlyMine, displayCompleted }
          } ),
          map( ( { list, userId, onlyMine, displayCompleted } ) => {
            list = onlyMine ? list.filter( i => i.assignedTo.id === userId ) : list

            return { list, displayCompleted }
          } ),
          map( ( { list, displayCompleted } ) =>
            displayCompleted ? list : list.filter( i => !i.isFinalized )
          ),
        )
  }

  ngOnInit(): void {
    this.mProjectSubscription =
      this.projectFacade.GetCurrentProject$()
        .subscribe( project => {
          if( project != null ) {
            this.issuesFacade.LoadIssues()
          }
          else {
            this.issuesFacade.ClearIssues()
          }
        } )
  }

  ngOnDestroy() {
    this.mProjectSubscription?.unsubscribe()
    this.mProjectSubscription = undefined
  }
}
