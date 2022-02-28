using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MoreLinq;
using MvvmSupport.DialogService;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Common.Platform;
using SquirrelsNest.Common.Values;
using SquirrelsNest.Core.CompositeBuilders;
using SquirrelsNest.Desktop.Support;

namespace SquirrelsNest.Desktop.ViewModels {
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class EditIssueDialogViewModel : DialogAwareBase {
        public  const string                cIssueParameter = "issue";
        public  const string                cProjectParameter = "project";
        public  const string                cUserParameter = "user";

        private SnIssue ?                   mIssue;
        private CompositeProject ?          mProject;
        private string                      mTitle;
        private string                      mDescription;
        private SnIssueType                 mCurrentIssueType;
        private SnWorkflowState             mCurrentState;
        private SnComponent                 mCurrentComponent;
        private SnUser                      mAssignedUser;
        private SnUser ?                    mCurrentUser;

        public  ObservableCollection<SnIssueType>       IssueTypes { get; }
        public  ObservableCollection<SnWorkflowState>   WorkflowStates { get; }
        public  ObservableCollection<SnComponent>       Components { get; }
        public  ObservableCollection<SnUser>            Users { get; }

        public  string                      EntryInfo { get; private set; }
        public  string                      IssueNumber { get; private set; }

        public EditIssueDialogViewModel() {
            mTitle = String.Empty;
            mDescription = String.Empty;
            IssueTypes = new ObservableCollection<SnIssueType>();
            WorkflowStates = new ObservableCollection<SnWorkflowState>();
            Components = new ObservableCollection<SnComponent>();
            Users = new ObservableCollection<SnUser>();
            mCurrentIssueType = SnIssueType.Default;
            mCurrentState = SnWorkflowState.Default;
            mCurrentComponent = SnComponent.Default;
            mAssignedUser = SnUser.Default;
            mCurrentUser = SnUser.Default;

            SetTitle( "Issue Properties" );
            EntryInfo = String.Empty;
            IssueNumber = String.Empty;
        }

        public override void OnDialogOpened( IDialogParameters parameters ) {
            mIssue = parameters.GetValue<SnIssue>( cIssueParameter );
            mProject = parameters.GetValue<CompositeProject>( cProjectParameter );
            mCurrentUser = parameters.GetValue<SnUser>( cUserParameter );

            if( mProject == null ) {
                throw new ApplicationException( "A project parameter is required" );
            }
            if( mCurrentUser == null ) {
                throw new ApplicationException( "A user parameter is required" );
            }

            EntryInfo = $"Entered on {DateTimeProvider.Instance.CurrentDate.ToShortDateString()} by {mCurrentUser.Name}";        
            IssueNumber = $"{mProject.Project.IssuePrefix}-{mProject.Project.NextIssueNumber}";

            if( mIssue != null ) {
                IssueTitle = mIssue.Title;
                Description = mIssue.Description;

                var assignedTo = mProject.Users.FirstOrDefault( u => u.EntityId.Equals( mIssue.EnteredById ), SnUser.Default );
                
                EntryInfo = assignedTo.EntityId != EntityId.Default ? 
                    $"Entered on {mIssue.EntryDate.ToShortDateString()} by {assignedTo.Name}" : 
                    $"Entered on {mIssue.EntryDate.ToShortDateString()}";

                IssueNumber = $"{mProject.Project.IssuePrefix}-{mIssue.IssueNumber}";
            }

            OnPropertyChanged( nameof( EntryInfo ));
            OnPropertyChanged( nameof( IssueNumber ));

            BuildEntityLists( mProject );
            if( mIssue != null ) {
                SetEntityStates( mIssue );
            }
        }

        private void BuildEntityLists( CompositeProject project ) {
            IssueTypes.Clear();
            project.IssueTypes.OrderBy( it => it.Name ).ForEach( IssueTypes.Add );
            IssueTypes.Add( SnIssueType.Default );
            CurrentIssueType = SnIssueType.Default;

            WorkflowStates.Clear();
            project.WorkflowStates.OrderBy( s => s.Name ).ForEach( WorkflowStates.Add );
            WorkflowStates.Add( SnWorkflowState.Default );
            CurrentState = mProject != null ? 
                mProject.WorkflowStates.FirstOrDefault( s => s.IsInitialState, SnWorkflowState.Default ) : 
                SnWorkflowState.Default;

            Components.Clear();
            project.Components.OrderBy( c => c.Name ).ForEach( Components.Add );
            Components.Add( SnComponent.Default );
            CurrentComponent = SnComponent.Default;

            Users.Clear();
            project.Users.OrderBy( u => u.Name ).ForEach( Users.Add );
            Users.Add( SnUser.Default );
            AssignedUser = SnUser.Default;
        }

        private void SetEntityStates( SnIssue forIssue ) {
            CurrentIssueType = IssueTypes.First( it => it.EntityId.Equals( forIssue.IssueTypeId ));
            CurrentState = WorkflowStates.First( s => s.EntityId.Equals( forIssue.WorkflowStateId ));
            CurrentComponent = Components.First( c => c.EntityId.Equals( forIssue.ComponentId ));
            AssignedUser = Users.First( u => u.EntityId.Equals( forIssue.AssignedToId ));
        }

        [Required( ErrorMessage = "Issue title is required" )]
        [MinLength( 3, ErrorMessage = "Issue titles must be a minimum of 3 characters")]
        [MaxLength( 100, ErrorMessage = "Issue titles must be less than 100 characters" )]
        public string IssueTitle {
            get => mTitle;
            set => SetProperty( ref mTitle, value, true );
        }

        public string Description {
            get => mDescription;
            set => SetProperty( ref mDescription, value, true );
        }

        public SnIssueType CurrentIssueType {
            get => mCurrentIssueType;
            set => SetProperty( ref mCurrentIssueType, value, true );
        }

        public SnWorkflowState CurrentState {
            get => mCurrentState;
            set => SetProperty( ref mCurrentState, value, true );
        }

        public SnComponent CurrentComponent {
            get => mCurrentComponent;
            set => SetProperty( ref mCurrentComponent, value, true );
        }

        public SnUser AssignedUser {
            get => mAssignedUser;
            set => SetProperty( ref mAssignedUser, value, true );
        }

        protected override void OnAccept() {
            ValidateAllProperties();

            if((!HasErrors ) &&
               ( mProject != null )) {
                var issue = mIssue ?? new SnIssue( IssueTitle, mProject.Project.NextIssueNumber, mProject.Project.EntityId );

                issue = issue
                    .With( title: IssueTitle, description: Description, assignedTo: AssignedUser.EntityId, enteredBy: mCurrentUser?.EntityId )
                    .With( CurrentIssueType )
                    .With( CurrentState )
                    .With( CurrentComponent );

                RaiseRequestClose( 
                    new DialogResult( ButtonResult.Ok, 
                        new DialogParameters {{ cIssueParameter, issue }, { cProjectParameter, mProject }}));
            }
        }
    }
}
