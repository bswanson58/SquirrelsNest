using System;
using System.ComponentModel.DataAnnotations;
using MvvmSupport.DialogService;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.Desktop.Support;

namespace SquirrelsNest.Desktop.ViewModels {
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class EditReleaseDialogViewModel : DialogAwareBase {
        public  const string    cReleaseParameter = "release";

        private SnRelease ?     mRelease;
        private string          mReleaseName;
        private string          mReleaseDescription;

        public EditReleaseDialogViewModel() {
            SetTitle( "Release Properties" );

            mReleaseName = String.Empty;
            mReleaseDescription = String.Empty;
        }

        public override void OnDialogOpened( IDialogParameters parameters ) {
            mRelease = parameters.GetValue<SnRelease>( cReleaseParameter );

            if( mRelease != null ) {
                Description = mRelease.Description;
                Name = mRelease.Version;
            }
        }

        [Required( ErrorMessage = "Name is required" )]
        [MinLength( 3, ErrorMessage = "Release names must be a minimum of 3 characters")]
        [MaxLength( 100, ErrorMessage = "Release names must be less than 100 characters" )]
        public string Name {
            get => mReleaseName;
            set => SetProperty( ref mReleaseName, value, true );
        }

        public string Description {
            get => mReleaseDescription;
            set => SetProperty( ref mReleaseDescription, value, true );
        }

        protected override void OnAccept() {
            ValidateAllProperties();

            if(!HasErrors ) {
                var release = mRelease ?? new SnRelease( Name );

                release = release.With( version: Name, description: Description );

                RaiseRequestClose( new DialogResult( ButtonResult.Ok, new DialogParameters {{ cReleaseParameter, release }}));
            }
        }
    }
}
