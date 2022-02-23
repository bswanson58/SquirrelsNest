using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MoreLinq;
using MvvmSupport.DialogService;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Core.CompositeBuilders;
using SquirrelsNest.Desktop.Support;

namespace SquirrelsNest.Desktop.ViewModels {
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class EditIssueDialogViewModel : DialogAwareBase {
        public  const string                cIssueParameter = "issue";
        public  const string                cProjectParameter = "project";

        private SnIssue ?                   mIssue;
        private CompositeProject ?          mProject;
        private string                      mTitle;
        private string                      mDescription;
        private SnIssueType                 mCurrentIssueType;
        private SnWorkflowState             mCurrentState;

        public  ObservableCollection<SnIssueType>       IssueTypes { get; }
        public  ObservableCollection<SnWorkflowState>   WorkflowStates { get; }

        public  string                      EntryInfo { get; private set; }

        public EditIssueDialogViewModel() {
            mTitle = String.Empty;
            mDescription = String.Empty;
            IssueTypes = new ObservableCollection<SnIssueType>();
            WorkflowStates = new ObservableCollection<SnWorkflowState>();
            mCurrentIssueType = SnIssueType.Default;
            mCurrentState = SnWorkflowState.Default;

            SetTitle( "Issue Properties" );
            EntryInfo = String.Empty;
        }

        public override void OnDialogOpened( IDialogParameters parameters ) {
            mIssue = parameters.GetValue<SnIssue>( cIssueParameter );
            mProject = parameters.GetValue<CompositeProject>( cProjectParameter );

            if( mProject == null ) {
                throw new ApplicationException( "A project parameter is required" );
            }

            if( mIssue != null ) {
                IssueTitle = mIssue.Title;
                Description = mIssue.Description;

                EntryInfo = $"Entered on {mIssue.EntryDate.ToShortDateString()}";
                OnPropertyChanged( nameof( EntryInfo ));
            }

            BuildEntityLists( mProject );
            if( mIssue != null ) {
                SetEntityStates( mIssue );
            }
            else {
                SetDefaultStates();
            }
        }

        private void BuildEntityLists( CompositeProject project ) {
            IssueTypes.Clear();
            project.IssueTypes.OrderBy( it => it.Name ).ForEach( IssueTypes.Add );
            IssueTypes.Add( SnIssueType.Default );

            WorkflowStates.Clear();
            project.WorkflowStates.OrderBy( s => s.Name ).ForEach( WorkflowStates.Add );
            WorkflowStates.Add( SnWorkflowState.Default );
        }

        private void SetEntityStates( SnIssue forIssue ) {
            CurrentIssueType = IssueTypes.First( it => it.EntityId.Equals( forIssue.IssueTypeId ));
            CurrentState = WorkflowStates.First( s => s.EntityId.Equals( forIssue.WorkflowStateId ));
        }

        private void SetDefaultStates() {
            CurrentIssueType = SnIssueType.Default;
            CurrentState = SnWorkflowState.Default;
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

        protected override void OnAccept() {
            ValidateAllProperties();

            if((!HasErrors ) &&
               ( mProject != null )) {
                var issue = mIssue ?? new SnIssue( IssueTitle, mProject.Project.NextIssueNumber, mProject.Project.EntityId );

                issue = issue
                    .With( title: IssueTitle, description: Description )
                    .With( CurrentIssueType )
                    .With( CurrentState );

                RaiseRequestClose( 
                    new DialogResult( ButtonResult.Ok, 
                        new DialogParameters {{ cIssueParameter, issue }, { cProjectParameter, mProject }}));
            }
        }
    }
}
