using System;
using System.ComponentModel.DataAnnotations;
using MvvmSupport.DialogService;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Desktop.Support;

namespace SquirrelsNest.Desktop.ViewModels {
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class EditIssueTypeDialogViewModel : DialogAwareBase {
        public  const string    cIssueTypeParameter = "issueType";

        private SnIssueType ?   mIssueState;
        private string          mIssueTypeName;
        private string          mIssueTypeDescription;

        public EditIssueTypeDialogViewModel() {
            SetTitle( "Issue Type Properties" );

            mIssueTypeName = String.Empty;
            mIssueTypeDescription = String.Empty;
        }

        public override void OnDialogOpened( IDialogParameters parameters ) {
            mIssueState = parameters.GetValue<SnIssueType>( cIssueTypeParameter );

            if( mIssueState != null ) {
                mIssueTypeDescription = mIssueState.Description;
                mIssueTypeName = mIssueState.Name;
            }
        }

        [Required( ErrorMessage = "Name is required" )]
        [MinLength( 3, ErrorMessage = "Issue Type names must be a minimum of 3 characters")]
        [MaxLength( 100, ErrorMessage = "Issue Type names must be less than 100 characters" )]
        public string Name {
            get => mIssueTypeName;
            set => SetProperty( ref mIssueTypeName, value, true );
        }

        public string Description {
            get => mIssueTypeDescription;
            set => SetProperty( ref mIssueTypeDescription, value, true );
        }

        protected override void OnAccept() {
            if(!HasErrors ) {
                var issueType = mIssueState ?? new SnIssueType( Name );

                issueType = issueType.With( name: Name, description: Description );

                RaiseRequestClose( new DialogResult( ButtonResult.Ok, new DialogParameters {{ cIssueTypeParameter, issueType }}));
            }
        }
    }
}
