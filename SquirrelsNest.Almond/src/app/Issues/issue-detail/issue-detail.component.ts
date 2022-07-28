import {Component, Input, OnInit} from '@angular/core'
import {ClIssue} from '../../Data/graphQlTypes'

@Component( {
  selector: 'sn-issue-detail',
  templateUrl: './issue-detail.component.html',
  styleUrls: ['./issue-detail.component.css']
} )
export class IssueDetailComponent implements OnInit {
  @Input() issue!: ClIssue

  constructor() {
  }

  ngOnInit(): void {
  }

}
