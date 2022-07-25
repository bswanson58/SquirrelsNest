import {Component, OnInit} from '@angular/core'
import {Observable} from 'rxjs'
import {ClIssue} from '../../Data/graphQlTypes'
import {IssueService} from '../issues.service'

@Component( {
  selector: 'sn-issue-list',
  templateUrl: './issue-list.component.html',
  styleUrls: ['./issue-list.component.css']
} )
export class IssueListComponent implements OnInit {
  issueList$: Observable<ClIssue[]>

  constructor( private issuesService: IssueService ) {
    this.issueList$ = new Observable<ClIssue[]>()
  }

  ngOnInit(): void {
  }

  onIssueSelected( issue: ClIssue ) {
  }
}
