import {Component, OnInit} from '@angular/core'
import {Observable} from 'rxjs'
import {ClProject} from '../../Data/graphQlTypes'
import {ProjectService} from '../projects.service'

@Component( {
  selector: 'sn-project-list',
  templateUrl: './project-list.component.html',
  styleUrls: ['./project-list.component.css']
} )
export class ProjectListComponent implements OnInit {
  projectList$: Observable<ClProject[]>

  constructor( private projectService: ProjectService ) {
    this.projectList$ = new Observable<ClProject[]>()
  }

  ngOnInit(): void {
    this.projectList$ = this.projectService.LoadProjects()
  }

  onProjectSelected( selected: ClProject ) {
    console.log( selected )
  }
}
