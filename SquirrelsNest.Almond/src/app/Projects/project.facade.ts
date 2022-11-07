import {Injectable} from '@angular/core'
import {MatDialog, MatDialogConfig} from '@angular/material/dialog'
import {Store} from '@ngrx/store'
import {firstValueFrom, map, Observable, take, tap} from 'rxjs'
import {
  AddProjectInput,
  ClComponent,
  ClIssueType,
  ClProject, ClProjectTemplate,
  ClUser,
  ClWorkflowState, CreateTemplateInput,
  ProjectDetailInput,
  UpdateProjectInput
} from '../Data/graphQlTypes'
import {ClearIssues} from '../Issues/issues.actions'
import {AppState} from '../Store/app.reducer'
import {getProjects, getProjectTemplates, getSelectedProject, getServerHasMoreProjects} from '../Store/app.selectors'
import {
  ConfirmDialogComponent,
  ConfirmDialogData,
  ConfirmDialogResult
} from '../UI/confirm-dialog/confirm-dialog.component'
import {
  ProjectCreateDialogComponent,
  ProjectCreateResult
} from './project-create-dialog/project-create-dialog.component'
import {CategoryValues, ProjectConstants} from './project.const'
import {ProjectDetailsService} from './project.details.service'
import {ProjectTransferService} from './project.transfer.service'
import {ClearProjects, SelectProject} from './projects.actions'
import {ProjectService} from './projects.service'

@Injectable( {
  providedIn: 'root'
} )
export class ProjectFacade {
  constructor( private store: Store<AppState>,
               private projectConstants: ProjectConstants,
               private projectService: ProjectService,
               private projectDetailsService: ProjectDetailsService,
               private projectTransferService: ProjectTransferService,
               private dialog: MatDialog ) {
  }

  CreateProject() {
    this.dialog
      .open( ProjectCreateDialogComponent )
      .afterClosed()
      .subscribe( ( result: ProjectCreateResult ) => {
        if( (result.accepted) &&
          (result.project !== null) ) {
          const projectInput: AddProjectInput = {
            title: result.project?.title,
            description: result.project?.description,
            issuePrefix: result.project?.issuePrefix,
            projectTemplate: result.project?.projectTemplate
          }

          this.projectService.AddProject( projectInput )
        }
      } )
  }

  UpdateProject( project: UpdateProjectInput ) {
    this.projectService.UpdateProject( project )
  }

  DeleteProject( project: ClProject ) {
    const dialogData: ConfirmDialogData = {
      prompt: 'Do you want to delete this project?',
      promptDetail: project.name
    }
    const dialogConfig = new MatDialogConfig()
    dialogConfig.data = dialogData

    this.dialog
      .open( ConfirmDialogComponent, dialogConfig )
      .afterClosed()
      .subscribe( ( result: ConfirmDialogResult ) => {
        if( result.accepted ) {
          this.projectService.DeleteProject( project )
        }
      } )
  }

  AddProjectDetail( details: ProjectDetailInput ) {
    this.projectDetailsService.AddProjectDetail( details )
  }

  UpdateProjectDetail( details: ProjectDetailInput ) {
    this.projectDetailsService.UpdateProjectDetail( details )
  }

  DeleteProjectDetail( details: ProjectDetailInput ) {
    this.projectDetailsService.DeleteProjectDetail( details )
  }

  GetWorkflowStateCategoryValues$(): Observable<CategoryValues[]> {
    return this.projectConstants.GetWorkflowStateCategoryValues$()
  }

  GetWorkflowStateCategoryValues(): CategoryValues[] {
    return this.projectConstants.GetWorkflowStateCategoryValues()
  }

  GetProjectList$(): Observable<ClProject[]> {
    return this.store.select( getProjects )
  }

  GetServerHasMoreProjects$(): Observable<boolean> {
    return this.store.select( getServerHasMoreProjects )
  }

  GetCurrentProject$(): Observable<ClProject | null> {
    return this.store.select( getSelectedProject )
  }

  GetCurrentProject(): Promise<ClProject | null> {
    return firstValueFrom( this.store.select( getSelectedProject ) )
  }

  GetCurrentProjectComponents$(): Observable<ClComponent[]> {
    return this.store.select( getSelectedProject )
      .pipe(
        map( project => project ? project.components : [] )
      )
  }

  GetCurrentProjectIssueTypes$(): Observable<ClIssueType[]> {
    return this.store.select( getSelectedProject )
      .pipe(
        map( project => project ? project.issueTypes : [] )
      )
  }

  GetCurrentProjectWorkflowStates$(): Observable<ClWorkflowState[]> {
    return this.store.select( getSelectedProject )
      .pipe(
        map( project => project ? project.workflowStates : [] )
      )
  }

  GetCurrentProjectUsers$(): Observable<ClUser[]> {
    return this.store.select( getSelectedProject )
      .pipe(
        map( project => project ? project.users : [] )
      )
  }

  GetProjectTemplates$(): Observable<ClProjectTemplate[]> {
    return this.store.select( getProjectTemplates )
  }

  CreateProjectTemplate( templateInput: CreateTemplateInput ): void {
    this.projectService.CreateProjectTemplate( templateInput )
  }

  LoadProjects(): void {
    this.projectService.LoadProjects()
  }

  LoadMoreProjects(): void {
    this.projectService.LoadMoreProjects()
  }

  async SelectProject( project: ClProject ): Promise<ClProject> {
    const currentProject = await this.GetCurrentProject()

    if( currentProject?.id !== project?.id ) {
      this.store.dispatch( new ClearIssues() )
      this.store.dispatch( new SelectProject( project ) )
    }

    return project
  }

  FindProject( projectId: string ): ClProject | null {
    let project: ClProject | null = null

    this.GetProjectList$()
      .pipe(
        take( 1 ),
        map( projectList => projectList.find( p => p.id === projectId ) )
      ).subscribe( p => project = p ?? null )

    return project
  }

  ClearState(): void {
    this.store.dispatch( new ClearProjects() )
  }

  DownloadProject( project: ClProject ): Observable<Blob> {
    return this.projectTransferService.DownloadProject( project )
  }

  UploadProject( projectName: string, formData: FormData ): Observable<any> {
    return this.projectTransferService.UploadProject( projectName, formData )
      .pipe(
        tap( _ => {
          this.LoadProjects()
        } )
      )
  }

  LoadProjectTemplates() {
    this.projectService.LoadProjectTemplates()
  }
}
