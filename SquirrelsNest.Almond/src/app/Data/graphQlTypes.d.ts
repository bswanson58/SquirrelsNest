import { gql } from 'apollo-angular';
export type Maybe<T> = T | null;
export type InputMaybe<T> = Maybe<T>;
export type Exact<T extends { [key: string]: unknown }> = { [K in keyof T]: T[K] };
export type MakeOptional<T, K extends keyof T> = Omit<T, K> & { [SubKey in K]?: Maybe<T[SubKey]> };
export type MakeMaybe<T, K extends keyof T> = Omit<T, K> & { [SubKey in K]: Maybe<T[SubKey]> };
/** All built-in and custom scalars, mapped to their actual values */
export type Scalars = {
  ID: string;
  String: string;
  Boolean: boolean;
  Int: number;
  Float: number;
  /** The `Date` scalar represents an ISO-8601 compliant date type. */
  Date: any;
  /** The `DateTime` scalar represents an ISO-8601 compliant date time type. */
  DateTime: any;
};

export type AddIssueInput = {
  description: Scalars['String'];
  issueTypeId: Scalars['String'];
  projectId: Scalars['String'];
  title: Scalars['String'];
};

export type AddIssuePayload = {
  __typename?: 'AddIssuePayload';
  errors: Array<MutationError>;
  issue?: Maybe<ClIssue>;
};

export enum ApplyPolicy {
  AfterResolver = 'AFTER_RESOLVER',
  BeforeResolver = 'BEFORE_RESOLVER'
}

export type BooleanOperationFilterInput = {
  eq?: InputMaybe<Scalars['Boolean']>;
  neq?: InputMaybe<Scalars['Boolean']>;
};

export type ClComponent = {
  __typename?: 'ClComponent';
  description: Scalars['String'];
  id: Scalars['String'];
  name: Scalars['String'];
  projectId: Scalars['String'];
};

export type ClComponentFilterInput = {
  and?: InputMaybe<Array<ClComponentFilterInput>>;
  description?: InputMaybe<StringOperationFilterInput>;
  id?: InputMaybe<StringOperationFilterInput>;
  name?: InputMaybe<StringOperationFilterInput>;
  or?: InputMaybe<Array<ClComponentFilterInput>>;
  projectId?: InputMaybe<StringOperationFilterInput>;
};

export type ClComponentSortInput = {
  description?: InputMaybe<SortEnumType>;
  id?: InputMaybe<SortEnumType>;
  name?: InputMaybe<SortEnumType>;
  projectId?: InputMaybe<SortEnumType>;
};

export type ClIssue = {
  __typename?: 'ClIssue';
  assignedTo: ClUser;
  component: ClComponent;
  description: Scalars['String'];
  enteredBy: ClUser;
  entryDate: Scalars['Date'];
  id: Scalars['String'];
  isFinalized: Scalars['Boolean'];
  issueNumber: Scalars['Int'];
  issueType: ClIssueType;
  project: ClProjectBase;
  release: ClRelease;
  title: Scalars['String'];
  workflowState: ClWorkflowState;
};

export type ClIssueCollectionSegment = {
  __typename?: 'ClIssueCollectionSegment';
  items?: Maybe<Array<ClIssue>>;
  /** Information to aid in pagination. */
  pageInfo: CollectionSegmentInfo;
  totalCount: Scalars['Int'];
};

export type ClIssueFilterInput = {
  and?: InputMaybe<Array<ClIssueFilterInput>>;
  assignedTo?: InputMaybe<ClUserFilterInput>;
  component?: InputMaybe<ClComponentFilterInput>;
  description?: InputMaybe<StringOperationFilterInput>;
  enteredBy?: InputMaybe<ClUserFilterInput>;
  entryDate?: InputMaybe<ComparableDateOnlyOperationFilterInput>;
  id?: InputMaybe<StringOperationFilterInput>;
  isFinalized?: InputMaybe<BooleanOperationFilterInput>;
  issueNumber?: InputMaybe<ComparableInt32OperationFilterInput>;
  issueType?: InputMaybe<ClIssueTypeFilterInput>;
  or?: InputMaybe<Array<ClIssueFilterInput>>;
  project?: InputMaybe<ClProjectBaseFilterInput>;
  release?: InputMaybe<ClReleaseFilterInput>;
  title?: InputMaybe<StringOperationFilterInput>;
  workflowState?: InputMaybe<ClWorkflowStateFilterInput>;
};

export type ClIssueSortInput = {
  assignedTo?: InputMaybe<ClUserSortInput>;
  component?: InputMaybe<ClComponentSortInput>;
  description?: InputMaybe<SortEnumType>;
  enteredBy?: InputMaybe<ClUserSortInput>;
  entryDate?: InputMaybe<SortEnumType>;
  id?: InputMaybe<SortEnumType>;
  isFinalized?: InputMaybe<SortEnumType>;
  issueNumber?: InputMaybe<SortEnumType>;
  issueType?: InputMaybe<ClIssueTypeSortInput>;
  project?: InputMaybe<ClProjectBaseSortInput>;
  release?: InputMaybe<ClReleaseSortInput>;
  title?: InputMaybe<SortEnumType>;
  workflowState?: InputMaybe<ClWorkflowStateSortInput>;
};

export type ClIssueType = {
  __typename?: 'ClIssueType';
  description: Scalars['String'];
  id: Scalars['String'];
  name: Scalars['String'];
  projectId: Scalars['String'];
};

export type ClIssueTypeFilterInput = {
  and?: InputMaybe<Array<ClIssueTypeFilterInput>>;
  description?: InputMaybe<StringOperationFilterInput>;
  id?: InputMaybe<StringOperationFilterInput>;
  name?: InputMaybe<StringOperationFilterInput>;
  or?: InputMaybe<Array<ClIssueTypeFilterInput>>;
  projectId?: InputMaybe<StringOperationFilterInput>;
};

export type ClIssueTypeSortInput = {
  description?: InputMaybe<SortEnumType>;
  id?: InputMaybe<SortEnumType>;
  name?: InputMaybe<SortEnumType>;
  projectId?: InputMaybe<SortEnumType>;
};

export type ClProject = {
  __typename?: 'ClProject';
  components: Array<ClComponent>;
  description: Scalars['String'];
  id: Scalars['String'];
  inception: Scalars['Date'];
  issuePrefix: Scalars['String'];
  issueTypes: Array<ClIssueType>;
  name: Scalars['String'];
  nextIssueNumber: Scalars['Int'];
  repositoryUrl: Scalars['String'];
  users: Array<ClUser>;
  workflowStates: Array<ClWorkflowState>;
};

export type ClProjectBase = {
  __typename?: 'ClProjectBase';
  description: Scalars['String'];
  id: Scalars['String'];
  inception: Scalars['Date'];
  name: Scalars['String'];
  repositoryUrl: Scalars['String'];
};

export type ClProjectBaseFilterInput = {
  and?: InputMaybe<Array<ClProjectBaseFilterInput>>;
  description?: InputMaybe<StringOperationFilterInput>;
  id?: InputMaybe<StringOperationFilterInput>;
  inception?: InputMaybe<ComparableDateOnlyOperationFilterInput>;
  name?: InputMaybe<StringOperationFilterInput>;
  or?: InputMaybe<Array<ClProjectBaseFilterInput>>;
  repositoryUrl?: InputMaybe<StringOperationFilterInput>;
};

export type ClProjectBaseSortInput = {
  description?: InputMaybe<SortEnumType>;
  id?: InputMaybe<SortEnumType>;
  inception?: InputMaybe<SortEnumType>;
  name?: InputMaybe<SortEnumType>;
  repositoryUrl?: InputMaybe<SortEnumType>;
};

export type ClProjectCollectionSegment = {
  __typename?: 'ClProjectCollectionSegment';
  items?: Maybe<Array<ClProject>>;
  /** Information to aid in pagination. */
  pageInfo: CollectionSegmentInfo;
  totalCount: Scalars['Int'];
};

export type ClProjectFilterInput = {
  and?: InputMaybe<Array<ClProjectFilterInput>>;
  components?: InputMaybe<ListFilterInputTypeOfClComponentFilterInput>;
  description?: InputMaybe<StringOperationFilterInput>;
  id?: InputMaybe<StringOperationFilterInput>;
  inception?: InputMaybe<ComparableDateOnlyOperationFilterInput>;
  issuePrefix?: InputMaybe<StringOperationFilterInput>;
  issueTypes?: InputMaybe<ListFilterInputTypeOfClIssueTypeFilterInput>;
  name?: InputMaybe<StringOperationFilterInput>;
  nextIssueNumber?: InputMaybe<ComparableInt32OperationFilterInput>;
  or?: InputMaybe<Array<ClProjectFilterInput>>;
  repositoryUrl?: InputMaybe<StringOperationFilterInput>;
  users?: InputMaybe<ListFilterInputTypeOfClUserFilterInput>;
  workflowStates?: InputMaybe<ListFilterInputTypeOfClWorkflowStateFilterInput>;
};

export type ClProjectSortInput = {
  description?: InputMaybe<SortEnumType>;
  id?: InputMaybe<SortEnumType>;
  inception?: InputMaybe<SortEnumType>;
  issuePrefix?: InputMaybe<SortEnumType>;
  name?: InputMaybe<SortEnumType>;
  nextIssueNumber?: InputMaybe<SortEnumType>;
  repositoryUrl?: InputMaybe<SortEnumType>;
};

export type ClRelease = {
  __typename?: 'ClRelease';
  description: Scalars['String'];
  id: Scalars['String'];
  name: Scalars['String'];
  projectId: Scalars['String'];
  releaseDate: Scalars['Date'];
  repositoryLabel: Scalars['String'];
};

export type ClReleaseFilterInput = {
  and?: InputMaybe<Array<ClReleaseFilterInput>>;
  description?: InputMaybe<StringOperationFilterInput>;
  id?: InputMaybe<StringOperationFilterInput>;
  name?: InputMaybe<StringOperationFilterInput>;
  or?: InputMaybe<Array<ClReleaseFilterInput>>;
  projectId?: InputMaybe<StringOperationFilterInput>;
  releaseDate?: InputMaybe<ComparableDateOnlyOperationFilterInput>;
  repositoryLabel?: InputMaybe<StringOperationFilterInput>;
};

export type ClReleaseSortInput = {
  description?: InputMaybe<SortEnumType>;
  id?: InputMaybe<SortEnumType>;
  name?: InputMaybe<SortEnumType>;
  projectId?: InputMaybe<SortEnumType>;
  releaseDate?: InputMaybe<SortEnumType>;
  repositoryLabel?: InputMaybe<SortEnumType>;
};

export type ClUser = {
  __typename?: 'ClUser';
  email: Scalars['String'];
  id: Scalars['String'];
  loginName: Scalars['String'];
  name: Scalars['String'];
};

export type ClUserFilterInput = {
  and?: InputMaybe<Array<ClUserFilterInput>>;
  email?: InputMaybe<StringOperationFilterInput>;
  id?: InputMaybe<StringOperationFilterInput>;
  loginName?: InputMaybe<StringOperationFilterInput>;
  name?: InputMaybe<StringOperationFilterInput>;
  or?: InputMaybe<Array<ClUserFilterInput>>;
};

export type ClUserSortInput = {
  email?: InputMaybe<SortEnumType>;
  id?: InputMaybe<SortEnumType>;
  loginName?: InputMaybe<SortEnumType>;
  name?: InputMaybe<SortEnumType>;
};

export type ClWorkflowState = {
  __typename?: 'ClWorkflowState';
  category: StateCategory;
  description: Scalars['String'];
  id: Scalars['String'];
  name: Scalars['String'];
  projectId: Scalars['String'];
};

export type ClWorkflowStateFilterInput = {
  and?: InputMaybe<Array<ClWorkflowStateFilterInput>>;
  category?: InputMaybe<StateCategoryOperationFilterInput>;
  description?: InputMaybe<StringOperationFilterInput>;
  id?: InputMaybe<StringOperationFilterInput>;
  name?: InputMaybe<StringOperationFilterInput>;
  or?: InputMaybe<Array<ClWorkflowStateFilterInput>>;
  projectId?: InputMaybe<StringOperationFilterInput>;
};

export type ClWorkflowStateSortInput = {
  category?: InputMaybe<SortEnumType>;
  description?: InputMaybe<SortEnumType>;
  id?: InputMaybe<SortEnumType>;
  name?: InputMaybe<SortEnumType>;
  projectId?: InputMaybe<SortEnumType>;
};

/** Information about the offset pagination. */
export type CollectionSegmentInfo = {
  __typename?: 'CollectionSegmentInfo';
  /** Indicates whether more items exist following the set defined by the clients arguments. */
  hasNextPage: Scalars['Boolean'];
  /** Indicates whether more items exist prior the set defined by the clients arguments. */
  hasPreviousPage: Scalars['Boolean'];
};

export type ComparableDateOnlyOperationFilterInput = {
  eq?: InputMaybe<Scalars['Date']>;
  gt?: InputMaybe<Scalars['Date']>;
  gte?: InputMaybe<Scalars['Date']>;
  in?: InputMaybe<Array<Scalars['Date']>>;
  lt?: InputMaybe<Scalars['Date']>;
  lte?: InputMaybe<Scalars['Date']>;
  neq?: InputMaybe<Scalars['Date']>;
  ngt?: InputMaybe<Scalars['Date']>;
  ngte?: InputMaybe<Scalars['Date']>;
  nin?: InputMaybe<Array<Scalars['Date']>>;
  nlt?: InputMaybe<Scalars['Date']>;
  nlte?: InputMaybe<Scalars['Date']>;
};

export type ComparableInt32OperationFilterInput = {
  eq?: InputMaybe<Scalars['Int']>;
  gt?: InputMaybe<Scalars['Int']>;
  gte?: InputMaybe<Scalars['Int']>;
  in?: InputMaybe<Array<Scalars['Int']>>;
  lt?: InputMaybe<Scalars['Int']>;
  lte?: InputMaybe<Scalars['Int']>;
  neq?: InputMaybe<Scalars['Int']>;
  ngt?: InputMaybe<Scalars['Int']>;
  ngte?: InputMaybe<Scalars['Int']>;
  nin?: InputMaybe<Array<Scalars['Int']>>;
  nlt?: InputMaybe<Scalars['Int']>;
  nlte?: InputMaybe<Scalars['Int']>;
};

export type DeleteIssueInput = {
  issueId: Scalars['String'];
};

export type DeleteIssuePayload = {
  __typename?: 'DeleteIssuePayload';
  errors: Array<MutationError>;
  issueId: Scalars['String'];
};

export enum IssueUpdatePath {
  AssignedToId = 'ASSIGNED_TO_ID',
  ComponentId = 'COMPONENT_ID',
  Description = 'DESCRIPTION',
  IssueTypeId = 'ISSUE_TYPE_ID',
  ReleaseId = 'RELEASE_ID',
  Title = 'TITLE',
  Unknown = 'UNKNOWN',
  WorkflowStateId = 'WORKFLOW_STATE_ID'
}

export type ListFilterInputTypeOfClComponentFilterInput = {
  all?: InputMaybe<ClComponentFilterInput>;
  any?: InputMaybe<Scalars['Boolean']>;
  none?: InputMaybe<ClComponentFilterInput>;
  some?: InputMaybe<ClComponentFilterInput>;
};

export type ListFilterInputTypeOfClIssueTypeFilterInput = {
  all?: InputMaybe<ClIssueTypeFilterInput>;
  any?: InputMaybe<Scalars['Boolean']>;
  none?: InputMaybe<ClIssueTypeFilterInput>;
  some?: InputMaybe<ClIssueTypeFilterInput>;
};

export type ListFilterInputTypeOfClUserFilterInput = {
  all?: InputMaybe<ClUserFilterInput>;
  any?: InputMaybe<Scalars['Boolean']>;
  none?: InputMaybe<ClUserFilterInput>;
  some?: InputMaybe<ClUserFilterInput>;
};

export type ListFilterInputTypeOfClWorkflowStateFilterInput = {
  all?: InputMaybe<ClWorkflowStateFilterInput>;
  any?: InputMaybe<Scalars['Boolean']>;
  none?: InputMaybe<ClWorkflowStateFilterInput>;
  some?: InputMaybe<ClWorkflowStateFilterInput>;
};

export type LoginInput = {
  email: Scalars['String'];
  password: Scalars['String'];
};

export type LoginPayload = {
  __typename?: 'LoginPayload';
  expiration: Scalars['DateTime'];
  token: Scalars['String'];
};

export type Mutation = {
  __typename?: 'Mutation';
  addIssue: AddIssuePayload;
  deleteIssue: DeleteIssuePayload;
  login: LoginPayload;
  updateIssue: UpdateIssuePayload;
};


export type MutationAddIssueArgs = {
  issueInput: AddIssueInput;
};


export type MutationDeleteIssueArgs = {
  deleteInput: DeleteIssueInput;
};


export type MutationLoginArgs = {
  loginInput: LoginInput;
};


export type MutationUpdateIssueArgs = {
  updateInput: UpdateIssueInput;
};

export type MutationError = {
  __typename?: 'MutationError';
  message: Scalars['String'];
  suggestion: Scalars['String'];
};

export type Query = {
  __typename?: 'Query';
  issueList?: Maybe<ClIssueCollectionSegment>;
  projectList?: Maybe<ClProjectCollectionSegment>;
};


export type QueryIssueListArgs = {
  order?: InputMaybe<Array<ClIssueSortInput>>;
  projectId: Scalars['ID'];
  skip?: InputMaybe<Scalars['Int']>;
  take?: InputMaybe<Scalars['Int']>;
  where?: InputMaybe<ClIssueFilterInput>;
};


export type QueryProjectListArgs = {
  order?: InputMaybe<Array<ClProjectSortInput>>;
  skip?: InputMaybe<Scalars['Int']>;
  take?: InputMaybe<Scalars['Int']>;
  where?: InputMaybe<ClProjectFilterInput>;
};

export enum SortEnumType {
  Asc = 'ASC',
  Desc = 'DESC'
}

export enum StateCategory {
  Completed = 'COMPLETED',
  Initial = 'INITIAL',
  Intermediate = 'INTERMEDIATE',
  Terminal = 'TERMINAL'
}

export type StateCategoryOperationFilterInput = {
  eq?: InputMaybe<StateCategory>;
  in?: InputMaybe<Array<StateCategory>>;
  neq?: InputMaybe<StateCategory>;
  nin?: InputMaybe<Array<StateCategory>>;
};

export type StringOperationFilterInput = {
  and?: InputMaybe<Array<StringOperationFilterInput>>;
  contains?: InputMaybe<Scalars['String']>;
  endsWith?: InputMaybe<Scalars['String']>;
  eq?: InputMaybe<Scalars['String']>;
  in?: InputMaybe<Array<InputMaybe<Scalars['String']>>>;
  ncontains?: InputMaybe<Scalars['String']>;
  nendsWith?: InputMaybe<Scalars['String']>;
  neq?: InputMaybe<Scalars['String']>;
  nin?: InputMaybe<Array<InputMaybe<Scalars['String']>>>;
  nstartsWith?: InputMaybe<Scalars['String']>;
  or?: InputMaybe<Array<StringOperationFilterInput>>;
  startsWith?: InputMaybe<Scalars['String']>;
};

export type UpdateIssueInput = {
  issueId: Scalars['String'];
  operations: Array<UpdateOperationInput>;
};

export type UpdateIssuePayload = {
  __typename?: 'UpdateIssuePayload';
  errors: Array<MutationError>;
  issue?: Maybe<ClIssue>;
};

export type UpdateOperationInput = {
  path: IssueUpdatePath;
  value: Scalars['String'];
};
