import {Component} from '@angular/core'
import {MatCheckboxChange} from '@angular/material/checkbox'
import {map, Observable} from 'rxjs'
import {ProjectFacade} from '../../Projects/project.facade'
import {UiFacade} from '../../UI/ui.facade'
import {IssuesFacade} from '../issues.facade'

@Component( {
  selector: 'sn-issue-footer',
  templateUrl: './issue-footer.component.html',
  styleUrls: ['./issue-footer.component.css']
} )
export class IssueFooterComponent {
  haveSelectedProject$: Observable<boolean>
  haveMoreIssues$: Observable<boolean>
  totalIssues$: Observable<number>
  loadedIssues$: Observable<number>
  displayCompletedIssues$: Observable<boolean>
  displayOnlyMyIssues$: Observable<boolean>

  constructor( private projectFacade: ProjectFacade, private issuesFacade: IssuesFacade, private uiFacade: UiFacade ) {
    this.displayCompletedIssues$ = uiFacade.GetDisplayCompletedIssues$()
    this.displayOnlyMyIssues$ = uiFacade.GetDisplayOnlyMyIssues$()
    this.haveMoreIssues$ = issuesFacade.GetServerHasMoreIssues$()
    this.totalIssues$ = this.issuesFacade.GetTotalIssues$()
    this.loadedIssues$ = this.issuesFacade.GetLoadedIssues$()
    this.haveSelectedProject$ =
      projectFacade.GetCurrentProject$()
        .pipe(
          map( project => {
            return !!project
          } )
        )
  }

  onLoadMoreIssues() {
    this.issuesFacade.LoadMoreIssues()
  }

  onDisplayCompletedIssues( $event: MatCheckboxChange ) {
    this.uiFacade.SetDisplayCompletedIssues( $event.checked )
  }

  onDisplayMyIssues( $event: MatCheckboxChange ) {
    this.uiFacade.SetDisplayOnlyMyIssues( $event.checked )
  }
}
