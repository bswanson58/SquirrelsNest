import {createFeatureSelector} from '@ngrx/store'
import {ClProject} from '../Data/graphQlTypes'

export interface ProjectState {
  projects: ClProject[],
  selectedProject: ClProject | null,
  isLoading: boolean
}

export const initialProjectState: ProjectState = {
  projects: [],
  selectedProject: null,
  isLoading: false
}

export const getProjectState = createFeatureSelector<ProjectState>( 'projects' )

export const getIsLoading = ( state: ProjectState ) => state.isLoading
export const getProjects = ( state: ProjectState ) => state.projects
export const getSelectedProject = ( state: ProjectState ) => state.selectedProject
