import {Component, OnInit} from '@angular/core'
import {Observable} from 'rxjs'
import {ClProject} from '../../Data/graphQlTypes'
import {ProjectFacade} from '../project.facade'

@Component( {
  selector: 'sn-project-list',
  templateUrl: './project-list.component.html',
  styleUrls: ['./project-list.component.css']
} )
export class ProjectListComponent implements OnInit {
  projectList$: Observable<ClProject[]>
  serverHasMoreProjects$: Observable<boolean>

  constructor( private projectFacade: ProjectFacade ) {
    this.projectList$ = new Observable<ClProject[]>()
    this.serverHasMoreProjects$ = new Observable<boolean>()
  }

  ngOnInit(): void {
    this.projectList$ = this.projectFacade.GetProjectList$()
    this.serverHasMoreProjects$ = this.projectFacade.GetServerHasMoreProjects()

    this.projectFacade.LoadProjects()
  }

  onRetrieveMoreProjects() {
    this.projectFacade.LoadMoreProjects()
  }

  onProjectSelected( selected: ClProject ) {
    this.projectFacade.SelectProject( selected )
  }
}
