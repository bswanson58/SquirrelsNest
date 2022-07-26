import {createSelector} from '@ngrx/store'

import * as fromAuth from '../Auth/auth.state'
import * as fromProject from '../Projects/project.state'

// Authentication
export const getAuthLoading = createSelector( fromAuth.getAuthState, fromAuth.getIsLoading )
export const getIsAuthenticated = createSelector( fromAuth.getAuthState, fromAuth.getIsAuthenticated )

// Projects
export const getProjectsLoading = createSelector( fromProject.getProjectState, fromProject.getIsLoading )
export const getProjects = createSelector( fromProject.getProjectState, fromProject.getProjects )
export const getSelectedProject = createSelector( fromProject.getProjectState, fromProject.getSelectedProject )
export const getProjectQueryState = createSelector( fromProject.getProjectState, fromProject.getProjectQueryState )
export const getServerHasMoreProjects = createSelector( fromProject.getProjectState, fromProject.getServerHasMoreProjects )
