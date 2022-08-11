import {Component, OnDestroy, OnInit} from '@angular/core'
import {combineLatest, map, Observable, Subscription, tap} from 'rxjs'
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

  serverHasMoreIssues$: Observable<boolean>
  private mProjectSubscription: Subscription | undefined

  constructor( private projectFacade: ProjectFacade, private issuesFacade: IssuesFacade, private uiFacade: UiFacade ) {
    this.serverHasMoreIssues$ = this.issuesFacade.GetServerHasMoreIssues$()

    this.issueList$ =
      combineLatest( [
        this.issuesFacade.GetCurrentIssuesList$(),
        uiFacade.GetDisplayOnlyMyIssues$(),
        uiFacade.GetDisplayCompletedIssues$()] )
        .pipe(
          map( ( [list, onlyMine, displayCompleted] ) => {
            return { list, onlyMine, displayCompleted }
          } ),
//        tap( ( { list, onlyMine, displayCompleted } ) => console.log( 'OnlyMine: ' + onlyMine ) ),
//        tap( ( { list, onlyMine, displayCompleted } ) => console.log( 'List Length: ' + list.length ) ),
          map( ( { list, onlyMine, displayCompleted } ) => {
            list = onlyMine ? list.filter( i => i.project != null ) : list

            return { list, displayCompleted }
          } ),
//        tap( ( { list, displayCompleted } ) => console.log( 'Show Completed: ' + displayCompleted ) ),
//        tap( ( { list, displayCompleted } ) => console.log( 'List Length: ' + list.length ) ),
          map( ( { list, displayCompleted } ) =>
            displayCompleted ? list : list.filter( i => i.workflowState.category !== 'COMPLETED' )
          ),
//        tap( list => console.log( 'List Length: ' + list.length ) ),
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
