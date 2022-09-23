import {createSelector} from '@ngrx/store'

import * as fromAuth from '../Auth/auth.state'
import * as fromProject from '../Projects/project.state'
import * as fromIssue from '../Issues/issues.state'
import * as fromUser from '../Users/user.state'
import * as fromUI from '../UI/ui.state'
import * as fromUserData from '../UserData/user.data.state'

// Authentication
export const getAuthLoading = createSelector( fromAuth.getAuthState, fromAuth.getIsLoading )
export const getIsAuthenticated = createSelector( fromAuth.getAuthState, fromAuth.getIsAuthenticated )
export const getAuthenticationClaims = createSelector( fromAuth.getAuthState, fromAuth.getAuthenticationClaims )

// Projects
export const getProjects = createSelector( fromProject.getProjectState, fromProject.getProjects )
export const getSelectedProject = createSelector( fromProject.getProjectState, fromProject.getSelectedProject )
export const getProjectQueryState = createSelector( fromProject.getProjectState, fromProject.getProjectQueryState )
export const getServerHasMoreProjects = createSelector( fromProject.getProjectState, fromProject.getServerHasMoreProjects )
export const getProjectTemplates = createSelector( fromProject.getProjectState, fromProject.getProjectTemplates )

// Issues
export const getIssues = createSelector( fromIssue.getIssueState, fromIssue.getIssues )
export const getIssueQueryState = createSelector( fromIssue.getIssueState, fromIssue.getIssueQueryState )
export const getServerHasMoreIssues = createSelector( fromIssue.getIssueState, fromIssue.getServerHasMoreIssues )
export const getTotalIssues = createSelector( fromIssue.getIssueState, fromIssue.getTotalIssues )
export const getLoadedIssues = createSelector( fromIssue.getIssueState, fromIssue.getLoadedIssues )

// Users
export const getUsersLoading = createSelector( fromUser.getUserState, fromUser.getIsLoading )
export const getUsers = createSelector( fromUser.getUserState, fromUser.getUsers )
export const getUserQueryState = createSelector( fromUser.getUserState, fromUser.getUserQueryState )
export const getServerHasMoreUsers = createSelector( fromUser.getUserState, fromUser.getServerHasMoreUsers )
export const getTotalUsers = createSelector( fromUser.getUserState, fromUser.getTotalUsers )
export const getLoadedUsers = createSelector( fromUser.getUserState, fromUser.getLoadedUsers )

// User Data
export const getLastUsedProject = createSelector( fromUserData.getUserDataState, fromUserData.getLastProject )

// UI
export const getIssueDisplayStyle = createSelector( fromUI.getUiState, fromUI.getIssueDisplayStyle )
export const getDisplayOnlyMyIssues = createSelector( fromUI.getUiState, fromUI.getDisplayOnlyMyIssues )
export const getDisplayCompletedIssues = createSelector( fromUI.getUiState, fromUI.getDisplayCompletedIssues )
export const getLastError = createSelector( fromUI.getUiState, fromUI.getLastError )
export const getServiceIsActive = createSelector( fromUI.getUiState, fromUI.getServiceIsActive )
export const getServiceActivity = createSelector( fromUI.getUiState, fromUI.getServiceActivity )
