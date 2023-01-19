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
        public  bool    ShouldDisplay { get; }

        public PaginationInformation() {
            PageCount = 1;
            ShouldDisplay = false;
        }

        public PaginationInformation( int pageCount ) {
            PageCount = pageCount;
            ShouldDisplay = true;
        }
    }

    public interface IIssueRetriever {
        IEnumerable<SnCompositeIssue>   IssueList();
        PaginationInformation           PaginationInformation { get; }

        public delegate void            IssueListChangedHandler( object sender, EventArgs args );
        public event                    IssueListChangedHandler OnIssueListChanged;
    }

    public class IssueRetriever : IIssueRetriever, IDisposable {
        private const int                       DisplayPageSize = 3;

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

            mActionSubscriber.SubscribeToAction<SetCurrentProjectAction>( this, OnSetCurrentProject );
            mActionSubscriber.SubscribeToAction<LoadIssueListSuccessAction>( this, OnIssuesLoaded );
            mActionSubscriber.SubscribeToAction<SetIssueListPageAction>( this, OnIssueListPageChanged );

            mActionSubscriber.SubscribeToAction<IssueDisplayCompleted>( this, OnIssueDisplayCompleted );
            mActionSubscriber.SubscribeToAction<IssueDisplayCompletedLast>( this, OnIssueDisplayCompletedLast );
            mActionSubscriber.SubscribeToAction<IssueDisplayMyAssigned>( this, OnIssueDisplayMyAssigned );

            mActionSubscriber.SubscribeToAction<AddIssueSuccess>( this, OnIssueAdded );
            mActionSubscriber.SubscribeToAction<DeleteIssueSuccess>( this, OnIssueDeleted );
            mActionSubscriber.SubscribeToAction<UpdateIssueSuccess>( this, OnIssueUpdated );

            if( mProjectState.Value.CurrentProject != null ) {
                BeginNewProject();
            }
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

        private void OnIssueAdded( AddIssueSuccess action ) {
            OnIssueListChanged?.Invoke( this, EventArgs.Empty );
        }

        private void OnIssueUpdated( UpdateIssueSuccess action ) {
            OnIssueListChanged?.Invoke( this, EventArgs.Empty );
        }

        private void OnIssueDeleted( DeleteIssueSuccess action ) {
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

            OnIssueListChanged?.Invoke( this, EventArgs.Empty );
        }

        private void UpdatePaginationInformation() {
            if( mIssueState.Value.PageInformation.TotalCount < DisplayPageSize ) {
                PaginationInformation = new PaginationInformation();
            }
            else {
                var total = (int)Math.Ceiling( FilteredList().Count() / (double)DisplayPageSize );

                if( mIssueState.Value.PageInformation.HasNext ) {
                    total += 1;
                }

                PaginationInformation = new PaginationInformation( total );
            }
        }

        public void Dispose() {
            mActionSubscriber.UnsubscribeFromAllActions( this );
        }
    }
}
