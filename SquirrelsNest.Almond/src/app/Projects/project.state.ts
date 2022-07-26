import {createFeatureSelector} from '@ngrx/store'
import {ClProject} from '../Data/graphQlTypes'

export interface ProjectQueryInfo {
  hasNextPage: boolean,
  hasPreviousPage: boolean,
  loadedProjects: number,
  totalProjects: number
}

export interface ProjectState {
  projects: ClProject[],
  selectedProject: ClProject | null,
  queryInfo: ProjectQueryInfo,
  isLoading: boolean
}

export const initialProjectState: ProjectState = {
  projects: [],
  selectedProject: null,
  queryInfo: { hasNextPage: false, hasPreviousPage: false, loadedProjects: 0, totalProjects: 0 },
  isLoading: false
}

export const getProjectState = createFeatureSelector<ProjectState>( 'projects' )

export const getIsLoading = ( state: ProjectState ) => state.isLoading
export const getProjects = ( state: ProjectState ) => state.projects
export const getSelectedProject = ( state: ProjectState ) => state.selectedProject
export const getProjectQueryState = ( state: ProjectState ) => state.queryInfo
export const getServerHasMoreProjects = ( state: ProjectState ) => state.queryInfo.hasNextPage
