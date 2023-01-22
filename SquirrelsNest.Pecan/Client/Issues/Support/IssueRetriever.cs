using System;
using System.Collections.Generic;
using System.Linq;
using Fluxor;
using SquirrelsNest.Pecan.Client.Issues.Actions;
using SquirrelsNest.Pecan.Client.Issues.Store;
using SquirrelsNest.Pecan.Client.Projects.Actions;
using SquirrelsNest.Pecan.Client.Projects.Store;
using SquirrelsNest.Pecan.Client.UserData.Actions;
using SquirrelsNest.Pecan.Client.UserData.Store;
using SquirrelsNest.Pecan.Shared.Dto;
using SquirrelsNest.Pecan.Shared.Entities;

namespace SquirrelsNest.Pecan.Client.Issues.Support {
    public class PaginationInformation {
        public  int     PageCount { get; }
        public  bool    OnlyFilteredIssues { get; }
        public  bool    NoIssues { get; }
        public  bool    DisplayPageControl { get; }

        public PaginationInformation() {
            PageCount = 1;
            DisplayPageControl = false;
        }

        public PaginationInformation( int pageCount, bool displayPageControl, bool noIssues, bool onlyFilteredIssues ) {
            PageCount = pageCount;
            DisplayPageControl = displayPageControl;
            NoIssues = noIssues;
            OnlyFilteredIssues = onlyFilteredIssues;
        }
    }

    public interface IIssueRetriever {
        void                            StartRetrieving();
        void                            EndRetrieving();

        IEnumerable<SnCompositeIssue>   IssueList();
        PaginationInformation           PaginationInformation { get; }

        public delegate void            IssueListChangedHandler( object sender, EventArgs args );
        public event                    IssueListChangedHandler OnIssueListChanged;
    }

    public class IssueRetriever : IIssueRetriever, IDisposable {
        private const int                       DisplayPageSize = 15;

        private readonly IState<ProjectState>   mProjectState;
        private readonly IState<IssueState>     mIssueState;
        private readonly IState<UserDataState>  mUserDataState;
        private readonly IssueFacade            mIssueFacade;
        private readonly IActionSubscriber      mActionSubscriber;

        public           PaginationInformation                     PaginationInformation { get; private set; }
        public  event    IIssueRetriever.IssueListChangedHandler ? OnIssueListChanged;

        public IssueRetriever( IState<ProjectState> projectState, IState<IssueState> issueState, IState<UserDataState> dataState,
                               IssueFacade issueFacade, IActionSubscriber actionSubscriber ) {
            mProjectState = projectState;
            mIssueState = issueState;
            mUserDataState = dataState;
            mIssueFacade = issueFacade;
            mActionSubscriber = actionSubscriber;

            PaginationInformation = new PaginationInformation();
        }

        public void StartRetrieving() {
            mActionSubscriber.SubscribeToAction<SetCurrentProjectAction>( this, OnSetCurrentProject );
            mActionSubscriber.SubscribeToAction<LoadIssueListSuccessAction>( this, OnIssuesLoaded );
            mActionSubscriber.SubscribeToAction<SetIssueListPageAction>( this, OnIssueListPageChanged );

            mActionSubscriber.SubscribeToAction<IssueDisplayCompleted>( this, OnIssueDisplayCompleted );
            mActionSubscriber.SubscribeToAction<IssueDisplayCompletedLast>( this, OnIssueDisplayCompletedLast );
            mActionSubscriber.SubscribeToAction<IssueDisplayMyAssigned>( this, OnIssueDisplayMyAssigned );

            mActionSubscriber.SubscribeToAction<AddIssueSuccess>( this, OnAddIssueSuccess );
            mActionSubscriber.SubscribeToAction<DeleteIssueSuccess>( this, OnDeleteIssueSuccess );
            mActionSubscriber.SubscribeToAction<UpdateIssueSuccess>( this, OnUpdateIssueSuccess );

            if( mProjectState.Value.CurrentProject != null ) {
                BeginNewProject();
            }
        }

        public void EndRetrieving() {
            mActionSubscriber.UnsubscribeFromAllActions( this );
        }

        private void OnSetCurrentProject( SetCurrentProjectAction action ) {
            if(!mIssueState.Value.CurrentProjectId.Equals( action.Project.EntityId )) {
                BeginNewProject();
            }
        }

        private void OnIssuesLoaded( LoadIssueListSuccessAction action ) {
            InsureAdequateIssuesLoaded();
        }

        private void OnIssueListPageChanged( SetIssueListPageAction action ) {
            InsureAdequateIssuesLoaded();
        }

        private void OnIssueDisplayCompleted( IssueDisplayCompleted action ) {
            InsureAdequateIssuesLoaded();
        }

        private void OnIssueDisplayCompletedLast( IssueDisplayCompletedLast action ) {
            InsureAdequateIssuesLoaded();
        }

        private void OnIssueDisplayMyAssigned( IssueDisplayMyAssigned action ) {
            InsureAdequateIssuesLoaded();
        }

        private void OnAddIssueSuccess( AddIssueSuccess action ) {
            UpdatePaginationInformation();
        }

        private void OnUpdateIssueSuccess( UpdateIssueSuccess action ) {
            UpdatePaginationInformation();
        }

        private void OnDeleteIssueSuccess( DeleteIssueSuccess action ) {
            OnIssueListChanged?.Invoke( this, EventArgs.Empty );
        }

        private IEnumerable<SnCompositeIssue> FilteredList() {
            var issueList = mIssueState.Value.Issues as IEnumerable<SnCompositeIssue>;

            if(!mUserDataState.Value.DisplayCompletedIssues ) {
                issueList = issueList
                    .Where( i => !IsIssueCompleted( i ));
            }

            if( mUserDataState.Value.DisplayCompletedIssuesLast ) {
                issueList = issueList.OrderBy( IsIssueCompleted );
            }

            return issueList;
        }

        public IEnumerable<SnCompositeIssue> IssueList() {
            var pageValue = mIssueState.Value.CurrentDisplayPage > 0 ? (int)mIssueState.Value.CurrentDisplayPage - 1 : 0;

            return FilteredList()
                .Skip( pageValue * DisplayPageSize )
                .Take( DisplayPageSize );
        }

        private bool IsIssueCompleted( SnCompositeIssue issue ) =>
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            issue.WorkflowState != null &&
            ( issue.WorkflowState.Category.Equals( StateCategory.Completed ) ||
              issue.WorkflowState.Category.Equals( StateCategory.Terminal ));

        private void BeginNewProject() {
            if( mProjectState.Value.CurrentProject != null ) {
                mIssueFacade.PrepareForNewProject( mProjectState.Value.CurrentProject.EntityId, PageInformation.Default, 1, 
                                                   Enumerable.Empty<SnCompositeIssue>());

                mIssueFacade.LoadIssues( mProjectState.Value.CurrentProject, new PageRequest( 1, DisplayPageSize ));
            }
        }

        private void InsureAdequateIssuesLoaded() {
            if(( mProjectState.Value.CurrentProject != null ) &&
               ( mIssueState.Value.PageInformation.HasNext )) {
                var needMoreIssues = ( mIssueState.Value.CurrentDisplayPage * DisplayPageSize ) > IssueList().Count();

                if( needMoreIssues ) {
                    var page = new PageRequest( mIssueState.Value.PageInformation.CurrentPage + 1, DisplayPageSize );

                    mIssueFacade.LoadIssues( mProjectState.Value.CurrentProject, page );
                }
            }

            UpdatePaginationInformation();
        }

        private void UpdatePaginationInformation() {
            var filteredListCount = FilteredList().Count();
            var total = (int)Math.Ceiling( filteredListCount / (double)DisplayPageSize );

            if( mIssueState.Value.PageInformation.HasNext ) {
                total += 1;
            }

            var anyIssues = mIssueState.Value.PageInformation.TotalCount > 0;
            var onlyFilteredIssues = ( filteredListCount == 0 ) && anyIssues;
            var displayPageControl = total > 1;

            if((!displayPageControl ) &&
               ( mIssueState.Value.CurrentDisplayPage > 1 )) {
                mIssueFacade.SetIssueListPage( 1 );
            }

            PaginationInformation = new PaginationInformation( total, displayPageControl, !anyIssues, onlyFilteredIssues );

            OnIssueListChanged?.Invoke( this, EventArgs.Empty );
        }

        public void Dispose() {
            mActionSubscriber.UnsubscribeFromAllActions( this );
        }
    }
}
