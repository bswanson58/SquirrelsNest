export enum StateCategory {
  INTERMEDIATE,
  INITIAL,
  TERMINAL,
  COMPLETED,
}

export interface ClWorkflowState {
  projectId: String
  name: String
  description: String
  category: StateCategory
  id: String
}

export interface ClRelease {
  projectId: String
  name: String
  description: String
  repositoryLabel: String
  releaseDate: Date
  id: String
}

export interface ClUser {
  name: String
  loginName: String
  email: String
  id: String
}

export interface ClIssueType {
  projectId: String
  name: String
  description: String
  id: String
}

export interface ClComponent {
  projectId: String
  name: String
  description: String
  id: String
}

export interface AllIssuesForProjectEdge {
  cursor: String
  node: ClIssue
}

export interface AllProjectsEdge {
  cursor: String
  node: ClProject
}

export interface PageInfo {
  hasNextPage: Boolean
  hasPreviousPage: Boolean
  startCursor: String
  endCursor: String
}

export interface AllIssuesForProjectQueryResult {
  allIssuesForProject: ClIssueCollectionSegment
}

export interface ClIssueCollectionSegment {
  pageInfo: CollectionSegmentInfo
  items: ClIssue[]
  totalCount: number
}

export interface CollectionSegmentInfo {
  hasNextPage: Boolean
  hasPreviousPage: Boolean
}

export interface AllProjectsQueryResult {
  allProjects: AllProjectsConnection
}

export interface AllProjectsConnection {
  pageInfo: PageInfo
  edges: AllProjectsEdge[]
  nodes: ClProject[]
  totalCount: number
}

export interface ClIssue {
  title: String
  description: String
  project: ClProject
  issueNumber: number
  entryDate: Date
  enteredBy: ClUser
  issueType: ClIssueType
  component: ClComponent
  release: ClRelease
  workflowState: ClWorkflowState
  assignedTo: ClUser
  id: String
}

export interface ClProject {
  name: String
  description: String
  inception: Date
  repositoryUrl: String
  issuePrefix: String
  nextIssueNumber: number
  id: String
}

export interface AllIssuesForProjectConnection {
  projectId: String
  first: number
  after: String
  last: number
  before: String
//    where: ClIssueFilterInput
//    order: ClIssueSortInput[]
}

export interface AllProjectsConnection {
  first: number
  after: String
  last: number
  before: String
}
