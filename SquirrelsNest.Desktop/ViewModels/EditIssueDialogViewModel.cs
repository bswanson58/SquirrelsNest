using System;
using System.ComponentModel.DataAnnotations;
using MvvmSupport.DialogService;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Desktop.Support;

namespace SquirrelsNest.Desktop.ViewModels {
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class EditIssueDialogViewModel : DialogAwareBase {
        public  const string        cIssueParameter = "issue";
        public  const string        cProjectParameter = "project";

        private SnIssue ?           mIssue;
        private SnProject ?         mProject;
        private string              mTitle;
        private string              mDescription;

        public EditIssueDialogViewModel() {
            mTitle = String.Empty;
            mDescription = String.Empty;

            SetTitle( "Issue Properties" );

        }

        public override void OnDialogOpened( IDialogParameters parameters ) {
            mIssue = parameters.GetValue<SnIssue>( cIssueParameter );
            mProject = parameters.GetValue<SnProject>( cProjectParameter );

            if( mProject == null ) {
                throw new ApplicationException( "A project parameter is required" );
            }

            if( mIssue != null ) {
                IssueTitle = mIssue.Title;
                Description = mIssue.Description;
            }
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

        protected override void OnAccept() {
            if((!HasErrors ) &&
               ( mProject != null )) {
                var issue = mIssue ?? new SnIssue( IssueTitle, mProject.NextIssueNumber, mProject.EntityId );

                issue = issue.With( title: IssueTitle, description: Description );

                RaiseRequestClose( 
                    new DialogResult( ButtonResult.Ok, 
                        new DialogParameters {{ cIssueParameter, issue }, { cProjectParameter, mProject }}));
            }
        }
    }
}
