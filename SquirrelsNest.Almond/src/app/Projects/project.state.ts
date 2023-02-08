import {createFeatureSelector} from '@ngrx/store'
import {ClProject, ClProjectTemplate} from '../Data/graphQlTypes'

export interface ProjectQueryInfo {
  hasNextPage: boolean,
  hasPreviousPage: boolean,
  loadedProjects: number,
  totalProjects: number
}

export const initialProjectQueryInfo: ProjectQueryInfo = {
  hasNextPage: false,
  hasPreviousPage: false,
  loadedProjects: 0,
  totalProjects: 0
}

export interface ProjectState {
  projects: ClProject[],
  projectTemplates: ClProjectTemplate[],
  selectedProject: ClProject | null,
  queryInfo: ProjectQueryInfo,
}

export const initialProjectState: ProjectState = {
  projects: [],
  projectTemplates: [],
  selectedProject: null,
  queryInfo: initialProjectQueryInfo,
}

export const getProjectState = createFeatureSelector<ProjectState>( 'projects' )

export const getProjects = ( state: ProjectState ) => state.projects
export const getSelectedProject = ( state: ProjectState ) => state.selectedProject
export const getProjectQueryState = ( state: ProjectState ) => state.queryInfo
export const getServerHasMoreProjects = ( state: ProjectState ) => state.queryInfo.hasNextPage
export const getProjectTemplates = ( state: ProjectState ) => state.projectTemplates
