import {Component, OnInit} from '@angular/core'
import {Store} from '@ngrx/store'
import {Observable} from 'rxjs'
import {ClProject} from '../../Data/graphQlTypes'
import {AppState} from '../../Store/app.reducer'
import {getProjects, getServerHasMoreProjects} from '../../Store/app.selectors'
import {SelectProject} from '../projects.actions'
import {ProjectService} from '../projects.service'

@Component( {
  selector: 'sn-project-list',
  templateUrl: './project-list.component.html',
  styleUrls: ['./project-list.component.css']
} )
export class ProjectListComponent implements OnInit {
  projectList$: Observable<ClProject[]>
  serverHasMoreProjects$: Observable<boolean>

  constructor( private projectService: ProjectService, private store: Store<AppState> ) {
    this.projectList$ = new Observable<ClProject[]>()
    this.serverHasMoreProjects$ = new Observable<boolean>()
  }

  ngOnInit(): void {
    this.projectList$ = this.store.select( getProjects )
    this.serverHasMoreProjects$ = this.store.select( getServerHasMoreProjects )

    this.projectService.LoadProjects()
  }

  onRetrieveMoreProjects() {
    this.projectService.LoadMoreProjects()
  }

  onProjectSelected( selected: ClProject ) {
    if( selected != null ) {
      this.store.dispatch( new SelectProject( selected ) )
    }
  }
}
