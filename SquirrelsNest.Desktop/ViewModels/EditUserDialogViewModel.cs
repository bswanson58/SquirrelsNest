using System;
using System.ComponentModel.DataAnnotations;
using MvvmSupport.DialogService;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Desktop.Support;

namespace SquirrelsNest.Desktop.ViewModels {
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class EditUserDialogViewModel : DialogAwareBase {
        public  const string    cUserParameter = "user";

        private SnUser ?        mUser;
        private string          mUserName;
        private string          mLoginName;

        public EditUserDialogViewModel() {
            SetTitle( "User Properties" );

            mUserName = String.Empty;
            mLoginName = String.Empty;
        }

        public override void OnDialogOpened( IDialogParameters parameters ) {
            mUser = parameters.GetValue<SnUser>( cUserParameter );

            if( mUser != null ) {
                mLoginName = mUser.LoginName;
                mUserName = mUser.Name;
            }
        }

        public string Name {
            get => mUserName;
            set => SetProperty( ref mUserName, value, true );
        }

        [Required( ErrorMessage = "Name is required" )]
        [MinLength( 3, ErrorMessage = "User names must be a minimum of 3 characters")]
        [MaxLength( 100, ErrorMessage = "User names must be less than 100 characters" )]
        public string LoginName {
            get => mLoginName;
            set => SetProperty( ref mLoginName, value, true );
        }

        protected override void OnAccept() {
            ValidateAllProperties();

            if(!HasErrors ) {
                var release = mUser ?? new SnUser( LoginName );

                release = release.With( displayName: Name );

                RaiseRequestClose( new DialogResult( ButtonResult.Ok, new DialogParameters {{ cUserParameter, release }}));
            }
        }
    }
}
