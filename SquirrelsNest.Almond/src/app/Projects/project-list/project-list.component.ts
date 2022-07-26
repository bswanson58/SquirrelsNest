import {Component, OnInit} from '@angular/core'
import {Store} from '@ngrx/store'
import {Observable} from 'rxjs'
import {ClProject} from '../../Data/graphQlTypes'
import {AppState} from '../../Store/app.reducer'
import {getProjects} from '../../Store/app.selectors'
import {ProjectService} from '../projects.service'

@Component( {
  selector: 'sn-project-list',
  templateUrl: './project-list.component.html',
  styleUrls: ['./project-list.component.css']
} )
export class ProjectListComponent implements OnInit {
  projectList$: Observable<ClProject[]>

  constructor( private projectService: ProjectService, private store: Store<AppState> ) {
    this.projectList$ = new Observable<ClProject[]>()
  }

  ngOnInit(): void {
    this.projectList$ = this.store.select( getProjects )

    this.projectService.LoadProjects()
  }

  onProjectSelected( selected: ClProject ) {
    console.log( selected )
  }
}
